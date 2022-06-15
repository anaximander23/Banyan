using Banyan;
using MediatR;

namespace BanyanDemo
{
    public partial class App : ApplicationCore
    {
        public App(IMediator mediator)
            : base(mediator)
        {
            InitializeComponent();
        }
    }
}