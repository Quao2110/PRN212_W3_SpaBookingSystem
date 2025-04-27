using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BLL.Service;
using DAL.Models;
using Microsoft.Win32;


namespace WPF_SBS.Admin
{
    public partial class TherapistManagementWindow : Window
    {
        private readonly TherapistService _therapistService;
        private List<DAL.Models.Therapist> _therapists;
        private DAL.Models.Therapist _currentTherapist;
        private bool _isEditing = false;

        public TherapistManagementWindow()
        {
            InitializeComponent();
            _therapistService = new TherapistService();
            LoadTherapists();

            // Gán sự kiện
            btnAddTherapist.Click += btnAddTherapist_Click;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;
            btnChooseFile.Click += btnChooseFile_Click;
        }

        private void LoadTherapists()
        {
            try
            {
                _therapists = _therapistService.GetAllTherapists();
                dgTherapists.ItemsSource = _therapists.Select(t => new
                {
                    t.Id,
                    Username = t.IdNavigation.Username,
                    FullName = t.IdNavigation.Fullname,
                    Phone = t.IdNavigation.Phone,
                    Email = t.IdNavigation.Email,
                    Experience = t.Experience,
                    Description = t.Description,
                    t.Image
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chuyên viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddTherapist_Click(object sender, RoutedEventArgs e)
        {
            _isEditing = false;
            _currentTherapist = null;
            ClearForm();
            gbNewTherapist.Visibility = Visibility.Visible;
            txtUsername.Focus();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                if (_isEditing && _currentTherapist != null)
                {
                    // Cập nhật thông tin User
                    _currentTherapist.IdNavigation.Username = txtUsername.Text;
                    _currentTherapist.IdNavigation.Fullname = txtFullName.Text;
                    _currentTherapist.IdNavigation.Phone = txtPhone.Text;
                    _currentTherapist.IdNavigation.Email = txtEmail.Text;
                    // Cập nhật thông tin Therapist
                    _currentTherapist.Experience = int.Parse(txtExperience.Text);
                    _currentTherapist.Description = txtDescription.Text;

                    _therapistService.UpdateTherapist(_currentTherapist);
                }
                else
                {
                    // Thêm chuyên viên mới
                    var user = new User
                    {
                        Username = txtUsername.Text,
                        Fullname = txtFullName.Text,
                        Phone = txtPhone.Text,
                        Email = txtEmail.Text,
                        Password = txtPassword.Password // Nên hash ở tầng Service
                    };
                    var therapist = new DAL.Models.Therapist
                    {
                        IdNavigation = user,
                        Experience = int.Parse(txtExperience.Text),
                        Description = txtDescription.Text
                    };
                    _therapistService.AddTherapist(therapist);
                }

                LoadTherapists();
                ClearForm();
                MessageBox.Show(_isEditing ? "Cập nhật thành công!" : "Thêm mới thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi {(_isEditing ? "cập nhật" : "thêm")} chuyên viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập username");
                return false;
            }

            if (!_isEditing && string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập password");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên chuyên viên");
                return false;
            }

            if (!int.TryParse(txtExperience.Text, out _))
            {
                MessageBox.Show("Kinh nghiệm phải là số nguyên");
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void btnChooseFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                txtFileName.Text = openFileDialog.FileName;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selected = (dynamic)((Button)sender).DataContext;
            int id = selected.Id;
            _currentTherapist = _therapists.FirstOrDefault(t => t.Id == id);
            if (_currentTherapist == null) return;

            _isEditing = true;

            txtUsername.Text = _currentTherapist.IdNavigation.Username;
            txtFullName.Text = _currentTherapist.IdNavigation.Fullname;
            txtPhone.Text = _currentTherapist.IdNavigation.Phone;
            txtEmail.Text = _currentTherapist.IdNavigation.Email;
            txtExperience.Text = _currentTherapist.Experience?.ToString() ?? "";
            txtDescription.Text = _currentTherapist.Description;

            txtPassword.Password = "";
            txtPassword.IsEnabled = false;

            gbNewTherapist.Visibility = Visibility.Visible;
            txtUsername.Focus();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = (dynamic)((Button)sender).DataContext;
            int id = selected.Id;
            var therapistToDelete = _therapists.FirstOrDefault(t => t.Id == id);
            if (therapistToDelete == null) return;

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa {therapistToDelete.IdNavigation.Fullname}?", "Xác nhận xóa", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _therapistService.DeleteTherapist(therapistToDelete.Id);
                    LoadTherapists();
                    MessageBox.Show("Đã xóa thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa chuyên viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearForm()
        {
            txtUsername.Text = "";
            txtPassword.Password = "";
            txtPassword.IsEnabled = true;
            txtFullName.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtExperience.Text = "";
            txtDescription.Text = "";
            txtFileName.Text = "Không có tệp nào được chọn";
            gbNewTherapist.Visibility = Visibility.Collapsed;
            _currentTherapist = null;
            _isEditing = false;
        }
    }
}