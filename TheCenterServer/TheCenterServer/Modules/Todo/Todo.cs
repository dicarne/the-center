using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheCenterServer.PModule
{
    [PModule("todos", "待办事项")]
    public class Todo : ModuleBase
    {
        [Persistence]
        public List<ToDoData> todos { get; set; } = new();

        [UI]
        public Group todoItems = new();
        [UI]
        public Button addNewButton = new("+", onClick: "AddNewClick");
        public override List<UICom> BuildInterface()
        {
            todoItems.children(todos.Select(t => new Group().children(new List<UICom>() {
                new CheckBox(t.active, onChange: "toggle").Flex("50px").ID("c:" + t.id),
                new Input(t.content, onChange: "changeItem")
                    .Style("border-top-color", "transparent")
                    .Style("border-left-color", "transparent")
                    .Style("border-right-color", "transparent")
                    .Flex("auto").ID("t:" + t.id),
                new Button("--", onClick: "toggle")
                    .Style("border-color", "transparent")
                    .Flex("50px").ID("r:" + t.id)
            }).Horizon(true).Spacing(8, 8).UI!).ToList());
            return new List<UICom>() {
                todoItems,
                addNewButton
            };
        }

        void AddNewTodo(string content)
        {
            SetState(() =>
            {
                todos.Insert(0, new ToDoData()
                {
                    active = false,
                    content = content,
                    id = Guid.NewGuid().ToString()
                });
                SyncUI();
            });
        }

        [Method]
        public void AddNewClick()
        {
            AddNewTodo("new!");
        }

        public class ToDoData
        {
            public bool active { get; set; }
            public string content { get; set; }
            public string id { get; set; }
        }

        public override void OnCustomEvent(string control, string eventName, string[]? args = null)
        {
            Console.WriteLine($"ctrl: {control}, event: {eventName}");
            if (control.StartsWith("t:"))
            {
                var id = control.Substring(2);
                SetState(() =>
                {
                    todos.Find(t => t.id == id)!.content = args![0];
                });
            }
            else if (control.StartsWith("r:"))
            {
                var id = control.Substring(2);
                SetState(() =>
                {
                    todos.RemoveAll(t => t.id == id);
                    SyncUI();
                });
            }
            else if (control.StartsWith("c:"))
            {
                var id = control.Substring(2);
                SetState(() =>
                {
                    var newv = args![0] == "true" ? true : false;
                    var index = todos.FindIndex(t => t.id == id);
                    var item = todos[index];
                    item.active = newv;
                    todos.RemoveAt(index);
                    if(newv)
                    {
                        var firstCompleteIndex = todos.FindIndex(t => t.active == true);
                        if(firstCompleteIndex != -1)
                        {
                            todos.Insert(firstCompleteIndex, item);
                        }
                        else
                        {
                            todos.Add(item);
                        }
                    }
                    else
                    {
                        todos.Insert(0, item);
                    }
                    SyncUI();
                });
            }
        }

        public override Task OnLoad()
        {
            BoardDesc.W = 11;
            return base.OnLoad();
        }
    }
}
