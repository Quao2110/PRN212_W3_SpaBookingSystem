using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BLL.Service;
using DAL.Models;
using System.Globalization;
using System.Text;

namespace WPF_SBS.Customer
{
    /// <summary>
    /// Interaction logic for Therapist.xaml
    /// </summary>
    public partial class Therapist : Window
    {
        private readonly TherapistService _therapistService;
        private List<DAL.Models.Therapist> _allTherapists;

        public Therapist()
        {
            InitializeComponent();
            _therapistService = new TherapistService();
            LoadTherapists();

            // Gán sự kiện
            txtSearch.TextChanged += FilterTherapists;
            cboExperience.SelectionChanged += FilterTherapists;
            btnSearch.Click += FilterTherapists;
        }

        private void LoadTherapists()
        {
            try
            {
                _allTherapists = _therapistService.GetAllTherapists();
                lstTherapists.ItemsSource = _allTherapists;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chuyên viên: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    // Đặc biệt cho Đ/đ
                    if (c == 'Đ') stringBuilder.Append('D');
                    else if (c == 'đ') stringBuilder.Append('d');
                    else stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private void FilterTherapists(object sender, RoutedEventArgs e)
        {
            if (_allTherapists == null) return;

            var searchText = RemoveDiacritics(txtSearch.Text.Trim().ToLower());
            foreach (var t in _allTherapists)
            {
                Console.WriteLine(RemoveDiacritics(t.IdNavigation.Fullname.ToLower()));
            }

            var filteredTherapists = _allTherapists.Where(t =>
                string.IsNullOrEmpty(searchText) ||
                (
                    t.IdNavigation.Fullname != null &&
                    RemoveDiacritics(t.IdNavigation.Fullname.Trim().ToLower()).Contains(searchText)
                )
            );

            // Nếu muốn kết hợp với lọc kinh nghiệm, giữ lại đoạn này:
            var selectedExperience = ((ComboBoxItem)cboExperience.SelectedItem)?.Content.ToString();
            if (!string.IsNullOrEmpty(selectedExperience) && selectedExperience != "Tất cả kinh nghiệm")
            {
                filteredTherapists = selectedExperience switch
                {
                    "Dưới 3 năm" => filteredTherapists.Where(t => t.Experience < 3),
                    "3-5 năm" => filteredTherapists.Where(t => t.Experience >= 3 && t.Experience <= 5),
                    "5-10 năm" => filteredTherapists.Where(t => t.Experience > 5 && t.Experience <= 10),
                    "Trên 10 năm" => filteredTherapists.Where(t => t.Experience > 10),
                    _ => filteredTherapists
                };
            }

            lstTherapists.ItemsSource = filteredTherapists.ToList();
        }

        private void btnViewDetail_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var therapist = (DAL.Models.Therapist)button.DataContext;

            // TODO: Mở cửa sổ chi tiết chuyên viên
            MessageBox.Show($"Xem chi tiết chuyên viên: {therapist.IdNavigation.Fullname}");
        }
    }
}
