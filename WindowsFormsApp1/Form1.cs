using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // Define a constant for the task file name
        private const string TASK_FILE_NAME = "tasks.csv";

        public Form1()
        {
            InitializeComponent();
            CenterToScreen();
        }

        public class Task
        {
            public string Name { get; set; }
            public DateTime Deadline { get; set; }
            public int Priority { get; set; }
            public Boolean IsCompleted { get; set; }
        }

        // Define list for the tasks to being manipulated and saved to the CSV file in the end
        public List<Task> tasksToSave = new List<Task>();

        // Utility function for sorting the tasks and returning a sorted list
        public List<Task> SortTasks(List<Task> tasks, int selectedIndex)
        {
            //Sort the list based on the selected index and return the sorted list
            if (selectedIndex == 0)
            {
                tasks.Sort((x, y) => string.Compare(x.Name, y.Name));
            }
            else if (selectedIndex == 1)
            {
                tasks.Sort((x, y) => x.Deadline.CompareTo(y.Deadline));
            }
            else
            {
                tasks.Sort((x, y) => y.Priority.CompareTo(x.Priority));
            }
            return tasks;
        }

        public List<Task> ManageTasks(List<Task> tasks, int selectedSortIndex, string textBoxValue, Boolean uncompletedOnly)
        {
            tasks = SortTasks(tasks, selectedSortIndex);

            List<Task> tasksToReturn = new List<Task>();
            // Loop through all the items in the list view
            foreach (Task task in tasks)
            {

                // Check if the name or priority of the task contains the text in the textBoxFilter
                if (task.Name.Contains(textBoxValue) || task.Priority.ToString().Contains(textBoxValue))
                {
                    // If the name or priority of the task contains the text in the textBoxFilter, add the task to the list
                    tasksToReturn.Add(task);
                }
            }

            if (uncompletedOnly)
            {
                tasksToReturn = tasksToReturn.Where(task => !task.IsCompleted).ToList();
            }

            return tasksToReturn;
        }

        public void ShowTaskFormModal(string name = "", int priority = 1, DateTime deadline = default)
        {
            // Create a form for editing the task
            Form taskForm = new Form
            {
                Text = deadline == default ? "Add Task" : "Edit Task",
                Width = 400,
                Height = 200
            };

            // Add a label and text box for the task name
            Label taskNameLabel = new Label
            {
                Text = "Task name:",
                Top = 20,
                Left = 20
            };
            taskForm.Controls.Add(taskNameLabel);

            TextBox taskNameTextBox = new TextBox
            {
                Top = 20,
                Left = 150,
                Width = 200,
                Text = name
            };
            taskForm.Controls.Add(taskNameTextBox);

            // Add a label and date picker for the deadline
            Label deadlineLabel = new Label
            {
                Text = "Deadline:",
                Top = 50,
                Left = 20
            };
            taskForm.Controls.Add(deadlineLabel);

            DateTimePicker deadlinePicker = new DateTimePicker
            {
                Top = 50,
                Left = 150,
                Width = 200,
                Value = deadline == default ? DateTime.Now : deadline
            };
            taskForm.Controls.Add(deadlinePicker);

            // Add a label and numeric up-down for the priority
            Label priorityLabel = new Label
            {
                Text = "Priority:",
                Top = 80,
                Left = 20
            };
            taskForm.Controls.Add(priorityLabel);

            NumericUpDown priorityUpDown = new NumericUpDown
            {
                Top = 80,
                Left = 150,
                Width = 200,
                Minimum = 1,
                Maximum = 10,
                Value = priority
            };
            taskForm.Controls.Add(priorityUpDown);

            // Add a submit button to the form
            Button submitButton = new Button
            {
                Text = "Submit",
                Top = 110,
                Left = 150
            };
            submitButton.Click += SubmitButtonOnClick;
            taskForm.Controls.Add(submitButton);
            taskForm.Tag = deadline == default ? "Add" : "Edit";

            taskForm.StartPosition = FormStartPosition.CenterScreen;
            // Show the form for editing the task
            taskForm.ShowDialog();
        }

        public void UpdateListView(List<Task> tasks)
        {
            // Clear the tasks from the list view
            listViewTasks.Items.Clear();

            // Loop through the tasks in the filtered list
            foreach (Task task in tasks)
            {
                // Add the task to the list view
                ListViewItem item = new ListViewItem(task.Name);
                item.SubItems.Add(task.Deadline.ToString("dd.MM.yyyy г."));
                item.SubItems.Add(task.Priority.ToString());
                if (task.IsCompleted)
                {
                    item.Tag = (task, "initialized");
                }
                else
                {
                    item.Tag = task;
                }
                item.Checked = task.IsCompleted;
                listViewTasks.Items.Add(item);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Create a list to store the tasks in CSV format
            List<string> tasks = new List<string>();

            // Check if the list view has any items
            if (tasksToSave.Count > 0)
            {
                // Loop through the tasks in the list view
                for (int i = 0; i < tasksToSave.Count; i++)
                {
                    // Add the task to the list in CSV format
                    tasks.Add($"{tasksToSave[i].Name},{tasksToSave[i].Deadline:yyyy-MM-dd},{tasksToSave[i].Priority},{tasksToSave[i].IsCompleted}");
                }

                // Write the tasks to the file
                File.WriteAllLines(TASK_FILE_NAME, tasks);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Add the columns to the list view
            listViewTasks.Columns.Add("Task Name", 250, HorizontalAlignment.Left);
            listViewTasks.Columns.Add("Deadline", 260, HorizontalAlignment.Center);
            listViewTasks.Columns.Add("Priority", 150, HorizontalAlignment.Center);


            // Check if the tasks file exists
            if (File.Exists(TASK_FILE_NAME))
            {
                // Read the tasks from the file
                string[] tasksFromFile = File.ReadAllLines(TASK_FILE_NAME);

                // Loop through the tasks from the file
                foreach (string task in tasksFromFile)
                {
                    // Split the task into its parts
                    string[] parts = task.Split(',');
                    string taskName = parts[0];
                    DateTime deadline = DateTime.Parse(parts[1]);
                    int priority = int.Parse(parts[2]);
                    Boolean isCompleted = Boolean.Parse(parts[3]);
                    // Create a new task object
                    Task taskObject = new Task
                    {
                        Name = taskName,
                        Deadline = deadline,
                        Priority = priority,
                        IsCompleted = isCompleted
                    };
                    tasksToSave.Add(taskObject);
                }

                UpdateListView(tasksToSave);

                comboBoxSort.SelectedIndex = 0;
            }
        }

        private void SubmitButtonOnClick(object sender, EventArgs e)
        {
            // Get the updated task details from the form
            Form taskForm = (Form)((Button)sender).Parent;
            string taskName = ((TextBox)taskForm.Controls[1]).Text;
            DateTime deadline = ((DateTimePicker)taskForm.Controls[3]).Value;
            if (deadline < DateTime.Now)
            {
                MessageBox.Show("Please enter a valid deadline that is later than or equal to the current date and time.");
                return;
            }
            int priority = (int)((NumericUpDown)taskForm.Controls[5]).Value;
            bool isEdit = (string)taskForm.Tag == "Edit";

            // Validate the input
            if (string.IsNullOrEmpty(taskName))
            {
                MessageBox.Show("Please enter a task name.");
                return;
            }

            if (priority < 1 || priority > 10)
            {
                MessageBox.Show("Please enter a valid priority (1-10).");
                return;
            }

            // Create a new task object
            Task task = new Task
            {
                Name = taskName,
                Deadline = deadline,
                Priority = priority,
                IsCompleted = false
            };

            // Get the selected task (if editing)
            ListViewItem item = null;
            if (isEdit && listViewTasks.SelectedItems.Count > 0)
            {
                item = listViewTasks.SelectedItems[0];
                for (int i = 0; i < tasksToSave.Count; i++)
                {
                    if (tasksToSave[i].Name == item.Text)
                    {
                        task.IsCompleted = item.Checked;
                        tasksToSave[i] = task;
                    }
                }
            }
            // Update the task in the list view (or add a new one)
            if (item == null)
            {
                item = new ListViewItem(task.Name);
                item.SubItems.Add(task.Deadline.ToString("dd.MM.yyyy г."));
                item.SubItems.Add(task.Priority.ToString());
                item.Tag = (task, "initialize");
                item.Checked = task.IsCompleted;
                listViewTasks.Items.Add(item);
                tasksToSave.Add(task);
            }
            else
            {
                item.Text = task.Name;
                item.SubItems[1].Text = task.Deadline.ToString("dd.MM.yyyy г.");
                item.SubItems[2].Text = task.Priority.ToString();
                item.Tag = task;
            }

            // Close the form
            taskForm.Close();
        }

        private void EditButtonOnClick(object sender, EventArgs e)
        {
            // Check if a task is selected
            if (listViewTasks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a task to edit.");
                return;
            }

            // Get the selected task
            ListViewItem item = listViewTasks.SelectedItems[0];
            Task task = (Task)item.Tag;

            // Check if the task object is null
            if (task == null)
            {
                // Show an error message and return
                MessageBox.Show("An error occurred while trying to edit the task. Please try again.");
                return;
            }

            ShowTaskFormModal(task.Name, task.Priority, task.Deadline);
        }

        private void DeleteButtonOnClick(object sender, EventArgs e)
        {
            // Check if an item is selected in the list view
            if (listViewTasks.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a task to delete.");
                return;
            }

            // Confirm the deletion
            DialogResult result = MessageBox.Show("Are you sure you want to delete the selected task?", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // Remove the selected item from list view and tasksToSave
                for (int i = 0; i < tasksToSave.Count; i++)
                {
                    if (tasksToSave[i].Name == listViewTasks.SelectedItems[0].Text)
                    {
                        tasksToSave.RemoveAt(i);
                        break;
                    }
                }
                listViewTasks.Items.Remove(listViewTasks.SelectedItems[0]);
            }
        }

        private void ListViewTasksOnItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Get the selected item
            ListViewItem item = listViewTasks.Items[e.Index];
            Task task = null;
            for (int i = 0; i < tasksToSave.Count; i++)
            {
                if (item.Text == tasksToSave[i].Name)
                {
                    task = tasksToSave[i];
                    break;
                }
            }
            if (item.Tag is Task)
            {
                task.IsCompleted = !task.IsCompleted;
            }
            else
            {
                item.Tag = task;
            }
        }

        private void AddButtonOnClick(object sender, EventArgs e)
        {
            ShowTaskFormModal();
        }

        private void SearchIconOnClick(object sender, EventArgs e)
        {
            textBoxFilter.Focus();
        }

        private void ComboBoxSortOnSelectedIndexChange(object sender, EventArgs e)
        {
            List<Task> tasks = ManageTasks(tasksToSave, comboBoxSort.SelectedIndex, textBoxFilter.Text, checkBoxUncompleted.Checked);

            UpdateListView(tasks);
        }

        private void TextBoxFilterOnTextChange(object sender, EventArgs e)
        {
            List<Task> tasks = ManageTasks(tasksToSave, comboBoxSort.SelectedIndex, textBoxFilter.Text, checkBoxUncompleted.Checked);

            UpdateListView(tasks);
        }

        private void CheckBoxUncompletedOnCheckedChange(object sender, EventArgs e)
        {
            List<Task> tasks = ManageTasks(tasksToSave, comboBoxSort.SelectedIndex, textBoxFilter.Text, checkBoxUncompleted.Checked);

            UpdateListView(tasks);
        }
    }
}