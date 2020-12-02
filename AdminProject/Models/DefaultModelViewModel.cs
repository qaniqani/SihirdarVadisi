using System.Collections.Generic;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Models
{
    public class DefaultModelViewModel
    {
        public PromiseDay PromiseDay { get; set; }
        public IList<Slider> Sliders { get; set; }
        public CategoryVideoItemViewModel DayOfVideo { get; set; }
        public List<SliderViewModel> SecondSlider { get; set; }
        public List<GameTypeContentViewModel> FourSubContent { get; set; }
        public List<CategoryVideoItemViewModel> LastedVideos { get; set; }
        public List<CategoryVideoItemViewModel> TopLastedVideos { get; set; }
    }
}