using System.Collections.Generic;

namespace AISandbox {
    public class StateMachine<T> {

        private List<State> m_states = new List<State>();
        private State m_currentState;
        private State m_previousState;

        public State PreviousState
        {
            get
            {
                return m_previousState;
            }
        }
        private T m_owner;

        public abstract class State {
            public abstract string Name { get; }
            public virtual void Enter(T t) {}
            public virtual void Update(T t) {}
            public virtual void Exit(T t) {}
            public abstract bool OnMessage(T t, Telegram msg);
        }

        public StateMachine(T t)
        {
            m_owner = t;
        }

        public bool AddState(State state) {
            m_states.Add(state);
            return true;
        }

        public bool RemoveState( State state ) {
            return m_states.Remove(state);           
        }

        public bool RemoveState(string name)
        {
            foreach (State state in m_states)
            {
                if (state.Name == name)
                {
                    return RemoveState(state);
                }
            }
            return false;
        }

        public bool SetActiveState( State state )
        {
            m_previousState = m_currentState;
            if (m_currentState != null)
            {
                m_currentState.Exit(m_owner);
            }
            m_currentState = state;
            m_currentState.Enter(m_owner);
            return true;
        }

        public bool SetActiveState(string name)
        {
            foreach (State state in m_states)
            {
                if (state.Name == name)
                {
                    return SetActiveState(state);
                }
            }
            return false;
        }

        public void Update() {
            m_currentState.Update(m_owner);
        }

        public bool HandleMessage(Telegram msg)
        {
            return (m_currentState != null && m_currentState.OnMessage(m_owner, msg));
        }
    }
}