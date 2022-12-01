using OnlineLibraryWPF.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Commands
{
    public class RentBookCommand : AsyncCommandBase
    {
        private RentedBooksService _rentedBooksService;

        public RentBookCommand(RentedBooksService rentedBooksService)
        {
            _rentedBooksService = rentedBooksService;
        }

        public override Task ExecuteAsync(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
