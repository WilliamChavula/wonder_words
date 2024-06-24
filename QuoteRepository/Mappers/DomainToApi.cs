using DomainModels;

namespace QuoteRepository.Mappers;

public static class DomainToApi
{
    public static string ToApiModel(this Tag tag)
    {
        return tag switch
        {
            Tag.Life => "life",
            Tag.Happiness => "happiness",
            Tag.Work => "work",
            Tag.Nature => "nature",
            Tag.Science => "science",
            Tag.Love => "love",
            Tag.Funny => "funny",
            _ => throw new ArgumentOutOfRangeException(nameof(tag), tag, null)
        };
    }
}