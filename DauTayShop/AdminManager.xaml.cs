using DauTayShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DauTayShop.Models;

// ...

namespace DauTayShop
{
    public partial class AdminManager : Window
    {
        private PRN221_ProjectContext dbContext;

        public AdminManager()
        {
            InitializeComponent();
            dbContext = new PRN221_ProjectContext();
            LoadFoodCategories();
            LoadFoods();
        }

        // CRUD FoodCategory
        //Search FoodCategory

        private void btnSearchCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchText = textBoxSearchCategory.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    LoadFoodCategories();
                }
                else
                {
                    List<FoodCategory> searchResults = dbContext.FoodCategories
                        .Where(fc => fc.Name.ToLower().Contains(searchText.ToLower()))
                        .ToList();
                    lvFoodCategory.ItemsSource = searchResults;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tìm kiếm danh mục: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //add FoodCategory
        private void btnAddFCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newCategoryName = txtFoodCategoryName.Text.Trim();

                if (string.IsNullOrWhiteSpace(newCategoryName))
                {
                    MessageBox.Show("Vui lòng nhập tên danh mục.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool categoryExists = dbContext.FoodCategories.Any(fc => fc.Name.Equals(newCategoryName));
                if (categoryExists)
                {
                    MessageBox.Show("Danh mục đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                FoodCategory newCategory = new FoodCategory { Name = newCategoryName };
                dbContext.FoodCategories.Add(newCategory);
                dbContext.SaveChanges();

                LoadFoodCategories();
                clearTextBox();

                MessageBox.Show("Thêm danh mục thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm danh mục: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //edit FoodCategory
        private void btnEditFCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFoodCategoryID.Text))
                {
                    MessageBox.Show("Vui lòng chọn một danh mục để cập nhật", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int selectedID = int.Parse(txtFoodCategoryID.Text);
                FoodCategory selectedCategory = dbContext.FoodCategories.FirstOrDefault(fc => fc.Id == selectedID);

                if (selectedCategory == null)
                {
                    MessageBox.Show("Danh mục không tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                selectedCategory.Name = txtFoodCategoryName.Text;
                dbContext.SaveChanges();

                LoadFoodCategories();
                clearTextBox();

                MessageBox.Show("Cập nhật danh mục thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //delete FoodCategory

        private void btnDeleteFCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFoodCategoryID.Text))
                {
                    MessageBox.Show("Vui lòng chọn một danh mục để tiến hành xóa.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int selectedID = int.Parse(txtFoodCategoryID.Text);
                FoodCategory selectedCategory = dbContext.FoodCategories.FirstOrDefault(fc => fc.Id == selectedID);

                if (selectedCategory == null)
                {
                    MessageBox.Show("Danh mục không tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa danh mục này cùng với các món ăn liên quan không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Xóa các Food liên quan trước
                    List<Food> relatedFoods = dbContext.Foods.Where(f => f.IdCategory == selectedID).ToList();
                    dbContext.Foods.RemoveRange(relatedFoods);

                    // Tiến hành xóa danh mục
                    dbContext.FoodCategories.Remove(selectedCategory);
                    dbContext.SaveChanges();

                    LoadFoodCategories();
                    clearTextBox();

                    MessageBox.Show("Xóa danh mục cùng với các món ăn liên quan thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);

                if (ex.InnerException != null)
                {
                    MessageBox.Show("Inner Exception: " + ex.InnerException.Message, "Lỗi nội bộ", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        //load FoodCategory
        private void LoadFoodCategories()
        {
            List<FoodCategory> foodCategories = dbContext.FoodCategories.ToList();
            lvFoodCategory.ItemsSource = foodCategories;
        }

        private void clearTextBox()
        {
            txtFoodCategoryID.Clear();
            txtFoodCategoryName.Clear();
        }

        private void btnRefreshFCategory_Click(object sender, RoutedEventArgs e)
        {
            clearTextBox();
        }

        //-----------------------FINISH CRUD FoodCategory----------------------------------------

        //CRUD Food

        //Load Food
        private void LoadFoods()
        {
            List<Food> foods = dbContext.Foods.Include(f => f.IdCategoryNavigation).ToList();
            lvFoods.ItemsSource = foods;

            // Load danh sách danh mục vào ComboBox
            List<FoodCategory> categories = dbContext.FoodCategories.ToList();
            cbFoodCategory.ItemsSource = categories;
            lvFoods.DisplayMemberPath = "IdCategoryNavigation.Name";
        }

        private void btnSearchFood_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtFoodSearch.Text.ToLower();
            List<Food> foods = dbContext.Foods.Include(f => f.IdCategoryNavigation)
                .Where(f => f.Name.ToLower().Contains(keyword))
                .ToList();

            lvFoods.ItemsSource = foods;
        }

        private void btnRefreshFood_Click(object sender, RoutedEventArgs e)
        {
            LoadFoods();
            ClearFoodFields();
        }

        private void ClearFoodFields()
        {
            txtFoodID.Clear();
            txtFoodName.Clear();
            txtFoodPrice.Clear();
            cbFoodCategory.ItemsSource = null;
            LoadFoods();
        }

        //Add Food
        private void btnAddFood_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string foodName = txtFoodName.Text.Trim();
                float foodPrice;

                if (!float.TryParse(txtFoodPrice.Text.Trim(), out foodPrice))
                {
                    MessageBox.Show("Vui lòng nhập giá món ăn hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                FoodCategory selectCategory = cbFoodCategory.SelectedItem as FoodCategory;

                if (selectCategory == null || string.IsNullOrEmpty(foodName) || foodPrice < 0)
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin hợp lệ cho món ăn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Trường hợp món ăn đã có trong hệ thống
                bool foodExists = dbContext.Foods.Any(f => f.Name.Equals(foodName));

                if (foodExists)
                {
                    MessageBox.Show("Món ăn đã tồn tại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Food newFood = new Food()
                {
                    Name = foodName,
                    Price = foodPrice,
                    IdCategory = selectCategory.Id
                };

                dbContext.Foods.Add(newFood);
                dbContext.SaveChanges();

                LoadFoods();
                ClearFoodFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnEditFood_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFoodID.Text))
                {
                    MessageBox.Show("Vui lòng chọn một món ăn để cập nhật.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int foodId = int.Parse(txtFoodID.Text);
                Food selectedFood = dbContext.Foods.FirstOrDefault(f => f.Id == foodId);

                if (selectedFood == null)
                {
                    MessageBox.Show("Món ăn không tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string foodName = txtFoodName.Text.Trim();
                float foodPrice;

                if (!float.TryParse(txtFoodPrice.Text.Trim(), out foodPrice) || foodPrice < 0)
                {
                    MessageBox.Show("Vui lòng nhập giá món ăn hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                FoodCategory selectCategory = cbFoodCategory.SelectedItem as FoodCategory;
                if (selectCategory == null || string.IsNullOrWhiteSpace(foodName))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin hợp lệ cho món ăn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Trường hợp món ăn đã có trong hệ thống với tên khác
                bool foodExists = dbContext.Foods.Any(f => f.Id != foodId && f.Name.Equals(foodName));
                if (foodExists)
                {
                    MessageBox.Show("Món ăn đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                selectedFood.Name = foodName;
                selectedFood.Price = foodPrice;
                selectedFood.IdCategory = selectCategory.Id;

                dbContext.SaveChanges();

                LoadFoods();
                ClearFoodFields();

                MessageBox.Show("Cập nhật thông tin món ăn thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnDeleteFood_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFoodID.Text))
                {
                    MessageBox.Show("Vui lòng chọn một món ăn để xóa.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int foodId = int.Parse(txtFoodID.Text);
                Food selectedFood = dbContext.Foods.FirstOrDefault(f => f.Id == foodId);

                if (selectedFood == null)
                {
                    MessageBox.Show("Món ăn không tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa món ăn này không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    dbContext.Foods.Remove(selectedFood);
                    dbContext.SaveChanges();

                    LoadFoods();
                    ClearFoodFields();

                    MessageBox.Show("Xóa món ăn thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //--------------CRUD Table-----------------
        // Load tables
        private void LoadTables()
        {
            List<TableFood> tables = dbContext.TableFoods.ToList();
            lvTables.ItemsSource = tables;
        }

        // Search tables
        //private void btnSearchTable_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        string searchText = txtTableSearch.Text.Trim();
        //        List<Table> searchResults = dbContext.TableFoods
        //            .Where(t => t.Name.Contains(searchText.ToLower))
        //            .ToList();

        //        lvTables.ItemsSource = searchResults;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Đã xảy ra lỗi khi tìm kiếm bàn: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        // Add table
        private void btnAddTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string tableName = txtTableName.Text.Trim();

                if (string.IsNullOrEmpty(tableName))
                {
                    MessageBox.Show("Vui lòng nhập tên bàn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Table newTable = new Table()
                {
                    Name = tableName,
                    Status = cbTableStatus.SelectedIndex
                };

                dbContext.TableFoods.Add(newTable);
                dbContext.SaveChanges();

                LoadTables();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm bàn: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Edit table
        private void btnEditTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTableID.Text))
                {
                    MessageBox.Show("Vui lòng chọn một bàn để cập nhật.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int tableId = int.Parse(txtTableID.Text);
                Table selectedTable = dbContext.Tables.FirstOrDefault(t => t.Id == tableId);

                if (selectedTable == null)
                {
                    MessageBox.Show("Bàn không tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string newTableName = txtTableName.Text.Trim();

                if (string.IsNullOrEmpty(newTableName))
                {
                    MessageBox.Show("Vui lòng nhập tên bàn mới.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                selectedTable.TableName = newTableName;
                dbContext.SaveChanges();

                LoadTables();
                ClearTableFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật bàn: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Delete table
        private void btnDeleteTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTableID.Text))
                {
                    MessageBox.Show("Vui lòng chọn một bàn để xóa.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int tableId = int.Parse(txtTableID.Text);
                Table selectedTable = dbContext.Tables.FirstOrDefault(t => t.Id == tableId);

                if (selectedTable == null)
                {
                    MessageBox.Show("Bàn không tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa bàn này không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    dbContext.Tables.Remove(selectedTable);
                    dbContext.SaveChanges();

                    LoadTables();
                    ClearTableFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi xóa bàn: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}

