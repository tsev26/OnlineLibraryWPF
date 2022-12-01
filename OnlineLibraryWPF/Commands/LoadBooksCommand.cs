using OnlineLibraryWPF.Models;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineLibraryWPF.Commands
{
    public class LoadBooksCommand : AsyncCommandBase
    {
        private BooksViewModel _viewModel;
        private BooksService _booksService;

        public LoadBooksCommand(BooksViewModel booksViewModel, BooksService booksService)
        {
            _viewModel = booksViewModel;
            _booksService = booksService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _viewModel.IsLoading = true;

            if (parameter is string searchString)
            {

            }
            else
            {
                searchString = "";
            }

            try
            {
                _viewModel.Books.Clear();
                List<Book> books = await _booksService.GetAllBooksAsync(searchString);

                foreach (Book book in books)
                {
                    _viewModel.Books.Add(book);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load books." + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _viewModel.IsLoading = false;
        }
    }
}
