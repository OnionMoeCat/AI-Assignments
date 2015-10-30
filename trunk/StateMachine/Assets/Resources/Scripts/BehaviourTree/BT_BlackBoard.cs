using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISandbox
{
    class BT_BlackBoard
    {
        Dictionary<string, object> _baseMemory = new Dictionary<string, object>();
        Dictionary<string, object> _treeMemory = new Dictionary<string, object>();
        public Dictionary<string, object> _getTreeMemory(string treeScope)
        {
            if (!_treeMemory.ContainsKey(treeScope))
            {
                Dictionary<string, object> temp = new Dictionary<string, object>();
                temp["nodeMemory"] = new Dictionary<string, object>();
                temp["openNode"] = new Dictionary<string, object>();
                _treeMemory[treeScope] = temp;
            }
            return _treeMemory[treeScope] as Dictionary<string, object>;
        }
        public Dictionary<string, object> _getNodeMemory(Dictionary<string, object> treeMemory, string nodeScope)
        {
            Dictionary<string, object> memory = treeMemory["nodeMemory"] as Dictionary<string, object>;
            if (memory[nodeScope] != null)
            {
                memory[nodeScope] = new Dictionary<string, object>();
            }
            return memory[nodeScope] as Dictionary<string, object>;
        }

        public Dictionary<string, object> _getMemory(string treeScope = null, string nodeScope = null)
        {
            var memory = _baseMemory;

            if (treeScope != null)
            {
                memory = this._getTreeMemory(treeScope);

                if (nodeScope != null)
                {
                    memory = this._getNodeMemory(memory, nodeScope);
                }
            }
            return memory;
        }

        public void Set(string key, object value, string treeScope = null, string nodeScope = null)
        {
            Dictionary<string, object> memory = this._getMemory(treeScope, nodeScope);
            memory[key] = value;
        }

        public object Get(string key, string treeScope = null, string nodeScope = null)
        {
            var memory = this._getMemory(treeScope, nodeScope);
            object value;
            if (!memory.TryGetValue(key, out value))
            {
                return null;
            }
            return value;
        }      
    }
}
