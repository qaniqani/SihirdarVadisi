using System.Collections.Generic;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface ISliderService : IBaseInterface<Slider>
    {
        IList<Slider> ActiveList();
        IList<Slider> SliderOrder();
        void SliderOrder(string[] order);
        IList<Slider> VideoOrder();
        void VideoOrder(string[] order);
        IList<Slider> ActiveTop4List();
        List<SliderViewModel> SecondSlider();
    }
}