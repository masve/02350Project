using _02350Project.ViewModel;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02350Project.Command
{
    class RemoveElementsCommand : IUndoRedoCommand
    {

        List<RemoveEdgeCommand> _removeEdgeCommands;
        List<RemoveNodeCommand> _removeNodeCommands;

        public RemoveElementsCommand(List<RemoveEdgeCommand> removeEdgeCommands, List<RemoveNodeCommand> removeNodeCommands)
        {
            _removeEdgeCommands = removeEdgeCommands;
            _removeNodeCommands = removeNodeCommands;
        }

        public void Execute()
        {
            foreach (RemoveEdgeCommand command in _removeEdgeCommands)
            {
                command.Execute();
            }

            foreach (RemoveNodeCommand command in _removeNodeCommands)
            {
                command.Execute();
            }
        }

        public void UnExecute()
        {
            foreach (RemoveNodeCommand command in _removeNodeCommands)
            {
                command.UnExecute();
            }

            foreach (RemoveEdgeCommand command in _removeEdgeCommands)
            {
                command.UnExecute();
            }
        }

    }
}
