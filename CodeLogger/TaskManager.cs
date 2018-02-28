using GenjiCore;
using GenjiCore.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeLogger
{
    public static class TaskManager
    {
        private const string DATA_TASK_PATH = "D:\\task.xml";
        public static List<ObjTask> ObjTasks { get; set; }
        //public static List<ObjTask> ObjTasks { get; private set; }
        public static ObjTask CurrentTask { get { return ObjTasks.FirstOrDefault(task => task.IsFocusing); } }
        public static void LoadTasks()
        {
            string xmlTaskData = string.Empty;
            ObjTasks = new List<ObjTask>();
            if (File.Exists(DATA_TASK_PATH))
            {
                xmlTaskData = File.ReadAllText(DATA_TASK_PATH);
                if (!xmlTaskData.IsNullOrEmpty())
                {
                    ObjTasks = xmlTaskData.ToObjectOf<List<ObjTask>>();
                    if (!ObjTasks.IsNull())
                        CoreControllerCenter.NotifyController
                            .ShowTaskbarPopup("Thông báo",
                            "Hoàn tất tải dữ liệu phiên làm việc",
                            () => { MessageBox.Show(ObjTasks.Count + " công việc đã được tải lên"); });
                    TasksLoaded();
                }
            }
        }
        public static ObjTask GetTaskInfo(Guid taskID)
        {
            ObjTask taskInfo = null;
            if (!ObjTasks.IsNull())
                taskInfo = ObjTasks.FirstOrDefault(task => task.TaskID == taskID);
            return taskInfo;
        }
        public static ObjTask AddTask(string taskName = "")
        {
            var task = new ObjTask() { TaskID = Guid.NewGuid() };
            if (!string.IsNullOrEmpty(taskName))
                task.Rename(taskName);

            ObjTasks.Add(task);
            TaskAdded(task);
            return task;
        }
        public static void RemoveTask(Guid taskID)
        {
            var task = ObjTasks.FirstOrDefault(objTask => objTask.TaskID == taskID);
            if (task != null)
                ObjTasks.Remove(task);
        }
        public static void SaveContext()
        {
            string xmlTaskData = ObjTasks.ToXmlString<List<ObjTask>>();
            File.WriteAllText(DATA_TASK_PATH, xmlTaskData);
        }
        public static void FocusTask(Guid taskID)
        {
            var task = ObjTasks.FirstOrDefault(t => t.TaskID == taskID);
            if (!task.IsNull())
            {
                if (!CurrentTask.IsNull())
                    CurrentTask.IsFocusing = false;
                task.IsFocusing = true;
            }
        }
        public static void ClickTask(Guid taskID, bool keepSelect)
        {
            if (!keepSelect)
                ObjTasks.Where(t => t.IsSelected).ToList().ForEach(t => t.IsSelected = false);
            var task = ObjTasks.FirstOrDefault(t => t.TaskID == taskID);
            if (task != null)
                task.IsSelected = !task.IsSelected;
        }

        public static Action<ObjTask> TaskAdded = (objTask) => { };
        public static Action<ObjTask> TaskRemoved = (objTask) => { };
        public static Action TasksLoaded = () => { };
        public static Action<ObjTask> CurrentChanged = (objTask) => { };
        public static Action<ObjTask, bool> SelectionChanged = (ObjTasks, isSelect) => { };
    }
}
