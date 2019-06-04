using AutoMapper;
using System.Linq;

namespace Garage25.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            ////Mappar från en StudentAddViewModel till en Student
            //CreateMap<StudentAddViewModel, Student>()
            //    .ForMember(dest => dest.Enrollments, opt => opt.Ignore())
            //    .ForPath(dest => dest.Address, opt => opt.MapFrom(model => new Address { City = model.City, Street = model.Street }));

            ////Mappar från en Student till en StudentDetailsViewModel
            //CreateMap<Student, StudentDetailsViewModel>()
            //    .ForMember(
            //        dest => dest.Courses,
            //        from => from.MapFrom(s => s.Enrollments.Select(c => c.Course).ToList()))
            //    .ForMember(
            //        dest => dest.Attending,
            //        from => from.MapFrom(p => p.Enrollments.Count));


        }
    }
}
