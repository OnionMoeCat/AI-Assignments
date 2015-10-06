using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AISandbox;

public class PathfollowingController : MonoBehaviour
{
    private Grid m_Grid;
    private GridNode m_target;
    List<GridNode> m_path;
    private int m_path_index;
    private IActor m_actor;
    private bool m_needToCalPath;

    public bool needToCalPath
    {
        get { return m_needToCalPath; }
        set { m_needToCalPath = value; }
    }

    // Use this for initialization
    void Start ()
    {
        m_Grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        m_target = m_Grid.GetRandGrid();
        m_actor = GetComponent<IActor>();
        m_path = new List<GridNode>();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    Vector2 steering = Vector2.zero;
        GridNode m_current = m_Grid.GetGridForPosition(transform.position);
        if (!m_target.Passable || m_current == m_target || m_path.Count == 0)
        {
            m_target = m_Grid.GetRandGrid();
            m_needToCalPath = true;
        }

	    if (m_current != null && m_needToCalPath)
	    {
	        m_needToCalPath = false;
            AStar.GetShortestPath(m_current, m_target, m_Grid.diagnoal, out m_path);
            m_path_index = 1;
        }

	    if (m_path.Count > 1)
	    {
            GridNode seeking = m_path[m_path_index];
	        if (m_current == seeking)
	        {
	            m_path_index += 1;
	            seeking = m_path[m_path_index];
	        }
	        if (seeking == m_target)
	        {
                Renderer renderer = seeking.GetComponent<Renderer>();
	            if (renderer != null)
	            {
                    steering = SteeringHelper.GetArriveSteering(m_actor, seeking.transform.position, renderer.bounds.size.x);
                }
	        }
	        else
	        {
                steering = SteeringHelper.GetSeekSteering(m_actor, seeking.transform.position);
            }
	    }

        m_actor.SetInput(steering.x, steering.y);
    }

}
