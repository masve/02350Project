using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350Project.Command
{
    class MoveNodesCommand : IUndoRedoCommand
    {

        private List<MoveNodeCommand> _commands = new List<MoveNodeCommand>();

        public MoveNodesCommand(List<MoveNodeCommand> commnads) {
            _commands = commnads;
        }

        public void Execute()
        {
            foreach (MoveNodeCommand c in _commands)
            {
                c.Execute();
            }
        }

        public void UnExecute()
        {
            foreach (MoveNodeCommand c in _commands)
            {
                c.UnExecute();
            }
        }
    }
}
