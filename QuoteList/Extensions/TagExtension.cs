using DomainModels;
using L10n = QuoteList.Resources.Resources;

namespace QuoteList.Extensions;

public static class TagExtension
{
    public static string ToLocalizedString(this Tag tag)
    {
        return tag switch
        {
            Tag.Life => L10n.lifeTagLabel,
            Tag.Happiness => L10n.happinessTagLabel,
            Tag.Work => L10n.workTagLabel,
            Tag.Nature => L10n.natureTagLabel,
            Tag.Science => L10n.scienceTagLabel,
            Tag.Love => L10n.loveTagLabel,
            Tag.Funny => L10n.funnyTagLabel,
            _ => throw new ArgumentOutOfRangeException(nameof(tag), tag, null)
        };
    }
}