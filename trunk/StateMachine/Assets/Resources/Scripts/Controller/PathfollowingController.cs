using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AISandbox;

public class PathfollowingController : MonoBehaviour
{
    private Grid m_grid;

    public Grid Grid
    {
        get
        {
            return m_grid;
        }
        set
        {
            m_grid = value;
        }
    }

    private int m_path_index;
    private IActor m_actor;

    private int[] m_keys;

    public int[] Keys
    {
        get { return m_keys; }
        set { m_keys = value; }
    }

    public int KeyNum
    {
        get
        {
            int num = 0;
            foreach (int key in Keys)
            {
                num += key;
            }
            return num;
        }
    }

    private StateMachine<PathfollowingController> m_statemachine;

    public StateMachine<PathfollowingController> StateMachine
    {
        get
        {
            return m_statemachine;
        }
        set
        {
            m_statemachine = value;
        }
    }

    public void Reset()
    {
        for (int i = 0; i < m_keys.Length; i++)
        {
            m_keys[i] = 0;
        }
    }

    public void Seek(GridNode i_seeking)
    {
        Vector2 steering = SteeringHelper.GetSeekSteering(m_actor, i_seeking.transform.position);
        m_actor.SetInput(steering.x, steering.y);
    }

    public void Arrive(GridNode i_seeking)
    {
        Vector2 steering = Vector2.zero;
        Renderer renderer = i_seeking.GetComponent<Renderer>();
        if (renderer != null)
        {
            steering = SteeringHelper.GetArriveSteering(m_actor, i_seeking.transform.position, renderer.bounds.size.x);
        }
        m_actor.SetInput(steering.x, steering.y);
    }

    public void Idle()
    {
        Vector2 steering = Vector2.zero;
        m_actor.SetInput(steering.x, steering.y);
    }

    public bool HandleMessage(Telegram msg)
    {
        return m_statemachine.HandleMessage(msg);
    }

    // Use this for initialization
    void Start ()
    {
        m_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        m_actor = GetComponent<IActor>();
        m_keys = new int[EntityColorIndex.GetColorLength()];

        m_statemachine = new StateMachine<PathfollowingController>(this);
        m_statemachine.AddState(new SeekKeyState());
        m_statemachine.AddState(new OpenDoorState());
        m_statemachine.AddState(new GetTreasureState());
        m_statemachine.AddState(new EndState());
        m_statemachine.SetActiveState("SeekKey");
    }
	
	// Update is called once per frame
	void Update ()
	{
        StateMachine.Update();
    }

}
