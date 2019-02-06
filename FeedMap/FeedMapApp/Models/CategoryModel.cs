using System;
using System.Collections.Generic;
using FeedMapApp.Models.Abstract;
using UIKit;
using System.Linq;
using System.Threading.Tasks;

namespace FeedMapApp.Models
{
    public class CategoryModel : UIPickerViewModel
    {
        private string[] _categories;
        RestService _restService;

        protected int _selectedIndex = 0;

        private UITextField _textField;

        public CategoryModel(UITextField field)
        {
            _textField = field;
            _restService = new RestService();
            TokenHeaderBuilder headerBuilder = new TokenHeaderBuilder(_restService);
            headerBuilder.BuildHeaders();

        }

        public async Task InitCat()
        {
            RestReturnObj<IEnumerable<FoodCategories>> categories =
                await _restService.GetAllFoodCategories();
            if (!categories.IsSuccess) return;
            _categories = categories.Obj.Select(fc => (string)fc.Name).ToArray();
        }

        public string SelectedItem
        {
            get { return _categories[_selectedIndex]; }
        }

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            return _categories.Length;
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            _textField.Text = _categories[row];
            return _categories[row];
        }

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            _textField.Text = _categories[row];
            _selectedIndex = (int)row;
        }
    }
}
