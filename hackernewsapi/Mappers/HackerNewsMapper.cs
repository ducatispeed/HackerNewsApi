using System;
using AutoMapper;
using hackernewsapi.Model;

namespace hackernewsapi.Profiles{

    public class HackerNewsMapper : Profile
    {
        public HackerNewsMapper(){
            CreateMap<StoryModel, OutputStoryModel>()
            .ForMember(dest => dest.uri, opt => opt.MapFrom(src => src.url))
            .ForMember(dest => dest.postedBy, opt => opt.MapFrom(src => src.by))
            .ForMember(dest => dest.commentCount, opt => opt.MapFrom(src => src.descendants))
            .ForMember(dest => dest.time, opt => opt.MapFrom(src => PrepareOutPutDate(src.time)));
        }

        private string PrepareOutPutDate(long uDate)
        {
            var dotNetDate = DateTimeOffset.FromUnixTimeSeconds(uDate);
            return dotNetDate.LocalDateTime.ToString("yyyy-MM-ddTHH:mm:ssK");
        }
    }
}