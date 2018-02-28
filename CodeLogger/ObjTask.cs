using GenjiCore.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLogger
{
    public class ObjTask
    {
        private const string DEFAULT_NAME = "NEW TASK";
        public ObjTask()
        {
            TaskID = Guid.NewGuid();
            TaskName = DEFAULT_NAME;
            FileAttachments = new List<string>();
            ListConfig = new List<ObjConfig>();
            ListContent = new List<ObjContent>();
            ListGrantStore = new List<ObjGrantStore>();
            ObjInfo = new ObjInfo();
            TaskState = TaskState.NotStarted;
        }

        [DisplayName("ID nhiệm vụ")]
        public Guid TaskID { get; set; }
        [DisplayName("Tên nhiệm vụ")]
        public string TaskName { get; set; }
        [DisplayName("Mô tả nhiệm vụ")]
        public string Content { get; set; }
        [DisplayName("Tệp tin đính kèm")]
        public List<string> FileAttachments { get; set; }
        [DisplayName("Trạng thái nhiệm vụ")]
        public TaskState TaskState { get; set; }
        [DisplayName("Bình luận")]
        public string Comments { get; set; }
        public List<ObjConfig> ListConfig { get; set; }
        public List<ObjContent> ListContent { get; set; }
        public List<ObjGrantStore> ListGrantStore { get; set; }
        public ObjInfo ObjInfo { get; set; }
        public void Rename(string name)
        {
            TaskName = name;
        }
        public void AttachFile(string fileName)
        {
            FileAttachments.Add(fileName);
        }
        private List<string> LoadAttachFile()
        {
            //....load file here
            return FileAttachments;
        }
        public List<string> GetAttachFiles()
        {
            if (FileAttachments.IsNull() || !FileAttachments.Any())
                LoadAttachFile();
            return FileAttachments;
        }
        public void UpdateState(TaskState state)
        {
            TaskState = state;
            TaskStateChanged(state);
        }
        private bool _IsSelected { get; set; }
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                SelecteStateChanged(value);
            }
        }
        public bool IsFocusing { get; set; }

        internal Action<TaskState> TaskStateChanged = (state) => { };
        internal Action<string> FileAdded = (fileName) => { };
        internal Action<string> TaskRenamed = (string name) => { };
        internal Action<bool> SelecteStateChanged = (isSelect) => { };
    }
}
