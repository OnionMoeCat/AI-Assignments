﻿using UnityEngine;
using System.Collections;

namespace AISandbox
{
    public class OrientedActor : MonoBehaviour, IActor
    {
        private const float MAX_SPEED = 10.0f;
        private const float STEERING_ACCEL = 50.0f;
        private const float STEERING_LINE_SCALE = 4.0f;

        private float max_speed = MAX_SPEED;

        public Vector2 initialVelocity = Vector2.zero;
        public bool wrapScreen = false;

        private Grid _grid = null;

        [SerializeField]
        private bool _DrawVectors = true;
        public bool DrawVectors
        {
            get
            {
                return _DrawVectors;
            }
            set
            {
                _DrawVectors = value;
                _steering_line.gameObject.SetActive(_DrawVectors);
            }
        }

        public LineRenderer _steering_line;

        private Renderer _renderer;
        private bool _screenWrapX = false;
        private bool _screenWrapY = false;

        private Vector2 _steering = Vector2.zero;
        private Vector2 _acceleration = Vector2.zero;
        private Vector2 _velocity = Vector2.zero;

        private int[] m_keys;

        public int[] Keys
        {
            get { return m_keys; }
            set { m_keys = value; }
        }

        private void Start()
        {
            _grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
            Debug.Assert(_grid != null);
            _renderer = GetComponent<Renderer>();
            _velocity = initialVelocity;
            DrawVectors = _DrawVectors;
        }
        public void Reset()
        {
            for (int i = 0; i < m_keys.Length; i++)
            {
                m_keys[i] = 0;
            }
        }

        public void SetInput(float x_axis, float y_axis)
        {
            _steering = Vector2.ClampMagnitude(new Vector2(x_axis, y_axis), 1.0f);
            _acceleration = _steering * STEERING_ACCEL;
        }

        public float TheoryMaxSpeed
        {
            get { return MAX_SPEED; }
        }

        public float MaxSpeed
        {
            get { return max_speed; }
        }

        public Vector2 Velocity
        {
            get { return _velocity; }
        }

        private Vector3 ScreenWrap()
        {
            Vector3 position = transform.position;
            if (wrapScreen)
            {
                if (_renderer.isVisible)
                {
                    _screenWrapX = false;
                    _screenWrapY = false;
                    return position;
                }
                else
                {
                    Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
                    if (!_screenWrapX && (viewportPosition.x > 1 || viewportPosition.x < 0))
                    {
                        position.x = -position.x;
                        _screenWrapX = true;
                    }
                    if (!_screenWrapY && (viewportPosition.y > 1 || viewportPosition.y < 0))
                    {
                        position.y = -position.y;
                        _screenWrapY = true;
                    }
                }
            }
            return position;
        }

        private void Update()
        {
            UpdateSteering();
            CheckGridNodeInventory();
        }

        private void UpdateSteering()
        {
            Vector3 position = ScreenWrap();
            _velocity += _acceleration * Time.deltaTime;
            float cost = _grid.GetGridCostForPosition(position);
            max_speed = MAX_SPEED / cost;
            _velocity = Vector2.ClampMagnitude(_velocity, max_speed);
            position += (Vector3)(_velocity * Time.deltaTime);
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.Normalize(_velocity));

            _steering_line.transform.rotation = Quaternion.identity;
            _steering_line.SetPosition(1, _steering * STEERING_LINE_SCALE);
            _steering_line.sortingOrder = 1;

            // The steering is reset every frame so SetInput() must be called every frame for continuous steering.
            _steering = Vector2.zero;
            _acceleration = Vector2.zero;
        }

        private void CheckGridNodeInventory()
        {
            GridNode gridNode = _grid.GetGridForPosition(transform.position);
            if (gridNode.EntityType == EntityType.Key)
            {
                int index = EntityColorIndex.GetIndex(gridNode.EntityColor);
                Debug.Assert(index >= 0 && index < EntityColorIndex.GetColorLength());
                //tell manager that key is taken
                Entity entity = EntityManager.Keys[index];
                entity.IsTaken = true;
                entity.GridNode.EntityType = EntityType.Nothing;
                Keys[index] += 1;
                return;
            }           
        }
    }
}