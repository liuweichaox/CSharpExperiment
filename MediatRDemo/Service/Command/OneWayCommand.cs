using MediatR;

namespace MediatRDemo.Service.Command
{
    public class OneWayCommand:IRequest 
    {
        public string Title { get; set; }
    }
}