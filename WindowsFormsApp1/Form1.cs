using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

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
        List<Task> tasksToSave = new List<Task>();

        // Utility function for sorting the tasks and returning a sorted list
        public List<Task> sortTasks(List<Task> tasks, int selectedIndex)
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
            // Set the list view's view mode to details and disable multiple selection
            listView1.View = View.Details;
            listView1.MultiSelect = false;
            comboBoxSort.SelectedIndex = 0;

            // Add the columns to the list view
            listView1.Columns.Add("Task Name", 250, HorizontalAlignment.Center);
            listView1.Columns.Add("Deadline", 260, HorizontalAlignment.Center);
            listView1.Columns.Add("Priority", 150, HorizontalAlignment.Center);


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
                    Task taskObject = new Task();
                    taskObject.Name = taskName;
                    taskObject.Deadline = deadline;
                    taskObject.Priority = priority;
                    taskObject.IsCompleted = isCompleted;
                    tasksToSave.Add(taskObject);
                }

                // Loop through the tasks in the list
                foreach (Task task in tasksToSave)
                {
                    // Add the task to the list view
                    ListViewItem item = new ListViewItem(task.Name);
                    item.SubItems.Add(task.Deadline.ToString());
                    item.SubItems.Add(task.Priority.ToString());
                    // Set the Checked property of the ListViewItem to the value of IsCompleted
                    item.Checked = task.IsCompleted;
                    if (item.Checked == false)
                    {
                        item.Tag = task;
                    }
                    else
                    {
                        item.Tag = (task, "initialize");
                    }

                    // Add the item to the ListView
                    listView1.Items.Add(item);
                }
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            // Get the updated task details from the form
            Form taskForm = (Form)((Button)sender).Parent;
            string taskName = ((TextBox)taskForm.Controls[1]).Text;
            DateTime deadline = ((DateTimePicker)taskForm.Controls[3]).Value;
            int priority = (int)((NumericUpDown)taskForm.Controls[5]).Value;

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
            Task task = new Task();
            task.Name = taskName;
            task.Deadline = deadline;
            task.Priority = priority;
            task.IsCompleted = false;

            // Get the selected task (if editing)
            ListViewItem item = null;
            if (listView1.SelectedItems.Count > 0)
            {
                item = listView1.SelectedItems[0];
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
                item.SubItems.Add(task.Deadline.ToString());
                item.SubItems.Add(task.Priority.ToString());
                item.Tag = (task, "initialize");
                item.Checked = task.IsCompleted;
                listView1.Items.Add(item);
                tasksToSave.Add(task);
            }
            else
            {
                item.Text = task.Name;
                item.SubItems[1].Text = task.Deadline.ToString();
                item.SubItems[2].Text = task.Priority.ToString();
                item.Tag = task;
            }

            // Close the form
            taskForm.Close();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            // Check if a task is selected
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a task to edit.");
                return;
            }

            // Get the selected task
            ListViewItem item = listView1.SelectedItems[0];
            Task task = (Task)item.Tag;

            // Check if the task object is null
            if (task == null)
            {
                // Show an error message and return
                MessageBox.Show("An error occurred while trying to edit the task. Please try again.");
                return;
            }

            // Create a form for editing the task
            Form taskForm = new Form();
            taskForm.Text = "Edit Task";
            taskForm.Width = 400;
            taskForm.Height = 200;

            // Add a label and text box for the task name
            Label taskNameLabel = new Label();
            taskNameLabel.Text = "Task Name:";
            taskNameLabel.Location = new Point(10, 10);
            taskForm.Controls.Add(taskNameLabel);
            taskForm.Controls.Add(new TextBox() { Name = "taskNameTextBox", Text = task.Name, Location = new Point(100, 10) });

            // Add a label and date picker for the deadline
            Label deadlineLabel = new Label();
            deadlineLabel.Text = "Deadline:";
            deadlineLabel.Location = new Point(10, 40);
            taskForm.Controls.Add(deadlineLabel);
            taskForm.Controls.Add(new DateTimePicker() { Name = "deadlineDatePicker", Value = task.Deadline, Location = new Point(100, 40) });

            // Add a label and numeric up-down for the priority
            Label priorityLabel = new Label();
            priorityLabel.Text = "Priority:";
            priorityLabel.Location = new Point(10, 70);
            taskForm.Controls.Add(priorityLabel);
            taskForm.Controls.Add(new NumericUpDown() { Name = "priorityNumericUpDown", Value = task.Priority, Minimum = 1, Maximum = 10, Location = new Point(100, 70) });

            // Add a submit button
            Button submitButton = new Button();
            submitButton.Text = "Submit";
            submitButton.Location = new Point(10, 100);
            submitButton.Click += new EventHandler(submitButton_Click);
            taskForm.Controls.Add(submitButton);

            // Show the form for editing the task
            taskForm.ShowDialog();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            // Create a form or dialog where the user can add a new task
            Form taskForm = new Form();
            taskForm.Text = "Add Task";
            taskForm.Width = 400;
            taskForm.Height = 200;
            // Add controls to the form for the task name, deadline, and priority
            Label taskNameLabel = new Label();
            taskNameLabel.Text = "Task name:";
            taskNameLabel.Top = 40;
            taskNameLabel.Left = 40;
            taskForm.Controls.Add(taskNameLabel);

            TextBox taskNameTextBox = new TextBox();
            taskNameTextBox.Top = 20;
            taskNameTextBox.Left = 150;
            taskNameTextBox.Width = 200;
            taskForm.Controls.Add(taskNameTextBox);

            Label deadlineLabel = new Label();
            deadlineLabel.Text = "Deadline:";
            deadlineLabel.Top = 50;
            deadlineLabel.Left = 20;
            taskForm.Controls.Add(deadlineLabel);

            DateTimePicker deadlinePicker = new DateTimePicker();
            deadlinePicker.Top = 50;
            deadlinePicker.Left = 150;
            deadlinePicker.Width = 200;
            taskForm.Controls.Add(deadlinePicker);

            Label priorityLabel = new Label();
            priorityLabel.Text = "Priority:";
            priorityLabel.Top = 80;
            priorityLabel.Left = 20;
            taskForm.Controls.Add(priorityLabel);

            NumericUpDown priorityUpDown = new NumericUpDown();
            priorityUpDown.Top = 80;
            priorityUpDown.Left = 150;
            priorityUpDown.Width = 200;
            priorityUpDown.Minimum = 1;
            priorityUpDown.Maximum = 10;
            taskForm.Controls.Add(priorityUpDown);

            // Add a submit button to the form
            Button submitButton = new Button();
            submitButton.Text = "Submit";
            submitButton.Top = 110;
            submitButton.Left = 150;
            submitButton.Click += submitButton_Click;
            taskForm.Controls.Add(submitButton);
            taskForm.StartPosition = FormStartPosition.CenterScreen;

            // Open the form
            taskForm.Show();
        }


        private void deleteButton_Click(object sender, EventArgs e)
        {
            // Check if an item is selected in the list view
            if (listView1.SelectedItems.Count == 0)
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
                    if (tasksToSave[i].Name == listView1.SelectedItems[0].Text)
                    {
                        tasksToSave.RemoveAt(i);
                        break;
                    }
                }
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
        }

        private void comboBoxSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected option from the ComboBox
            string selectedOption = this.comboBoxSort.SelectedItem.ToString();

            // Create a list to store the tasks in
            List<Task> tasks = new List<Task>();

            // Loop through the items in the list view
            foreach (ListViewItem item in this.listView1.Items)
            {
                // Get the task object from the list view item's Tag property
                Task task = (Task)item.Tag;

                // Add the task to the list
                tasks.Add(task);
            }
            // Sort the tasks based on the selected option
            if (selectedOption == "Name")
            {
                tasks.Sort((x, y) => string.Compare(x.Name, y.Name));
            }
            else if (selectedOption == "Deadline")
            {
                tasks.Sort((x, y) => x.Deadline.CompareTo(y.Deadline));
            }
            else if (selectedOption == "Priority")
            {
                tasks.Sort((x, y) => y.Priority.CompareTo(x.Priority));
            }

            // Clear the list view
            this.listView1.Items.Clear();

            // Add the sorted tasks to the list view
            foreach (Task task in tasks)
            {
                // Add the task to the list view
                ListViewItem item = new ListViewItem(task.Name);
                item.SubItems.Add(task.Deadline.ToString());
                item.SubItems.Add(task.Priority.ToString());
                item.Tag = task;
                this.listView1.Items.Add(item);
            }
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            // Create a list to store the tasks that match the filter criteria
            List<Task> filteredTasks = new List<Task>();

            // Loop through all the items in the list view
            foreach (Task task in tasksToSave)
            {

                // Check if the name or priority of the task contains the text in the textBoxFilter
                if (task.Name.Contains(textBoxFilter.Text) || task.Priority.ToString().Contains(textBoxFilter.Text))
                {
                    // If the name or priority of the task contains the text in the textBoxFilter, add the task to the list
                    filteredTasks.Add(task);
                }
            }

            // Clear the tasks from the list view
            listView1.Items.Clear();

            if (comboBoxSort.SelectedIndex >= 0)
            {
                filteredTasks = sortTasks(filteredTasks, comboBoxSort.SelectedIndex);
            }

            // Loop through the tasks in the filtered list
            foreach (Task task in filteredTasks)
            {
                // Add the task to the list view
                ListViewItem item = new ListViewItem(task.Name);
                item.SubItems.Add(task.Deadline.ToString());
                item.SubItems.Add(task.Priority.ToString());
                item.Tag = (task, "initialize");
                item.Checked = task.IsCompleted;
                listView1.Items.Add(item);
            }
        }

        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Get the selected item
            ListViewItem item = listView1.Items[e.Index];
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBoxFilter.Focus();
        }

        private void checkBoxUncompleted_CheckedChanged(object sender, EventArgs e)
        {
            // Get the check box that was checked or unchecked
            CheckBox checkBox = (CheckBox)sender;

            if (checkBox.Checked)
            {
                // Create a list to store the tasks that match the filter criteria
                List<Task> filteredTasks = new List<Task>();

                // Loop through the tasks in the tasksToSave list
                foreach (Task task in tasksToSave)
                {
                    // Check if the task is not completed
                    if (!task.IsCompleted)
                    {
                        // Add the task to the filtered list
                        filteredTasks.Add(task);
                    }
                }

                // Clear the list view
                listView1.Items.Clear();

                // Add the filtered tasks to the list view
                foreach (Task task in filteredTasks)
                {
                    // Add the task to the list view
                    ListViewItem item = new ListViewItem(task.Name);
                    item.SubItems.Add(task.Deadline.ToString());
                    item.SubItems.Add(task.Priority.ToString());
                    item.Tag = (task, "initialized");
                    item.Checked = task.IsCompleted;
                    listView1.Items.Add(item);
                }
            }
            else
            {
                // Clear the list view
                listView1.Items.Clear();

                // Loop through the tasks in the tasksToSave list
                foreach (Task task in tasksToSave)
                {
                    // Add the task to the list view
                    ListViewItem item = new ListViewItem(task.Name);
                    item.SubItems.Add(task.Deadline.ToString());
                    item.SubItems.Add(task.Priority.ToString());
                    item.Tag = (task, "initialized");
                    item.Checked = task.IsCompleted;
                    listView1.Items.Add(item);
                }
            }
        }

    }
}