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

    private IActor m_actor;

    void Start()
    {
        m_actor = GetComponent<IActor>();
        m_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
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
}
