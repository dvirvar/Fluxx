using System.Collections.Generic;
using System.Linq;

public readonly struct DeckData
{
    static DeckData()
    {
        ActionCards = EnumUtil.GetArrayOf<ActionCardType>().Select(type => new ActionCardInfo(type)).ToList().AsReadOnly();
        GoalCards = EnumUtil.GetArrayOf<GoalCardType>().Select(type => new GoalCardInfo(type)).ToList().AsReadOnly();
        KeeperCards = EnumUtil.GetArrayOf<KeeperCardType>().Select(type => new KeeperCardInfo(type)).ToList().AsReadOnly();
        NewRuleCards = EnumUtil.GetArrayOf<NewRuleCardType>().Select(type => new NewRuleCardInfo(type)).ToList().AsReadOnly();
    }
        

    public static readonly IReadOnlyList<ActionCardInfo> ActionCards;
    public static readonly IReadOnlyList<GoalCardInfo> GoalCards;
    public static readonly IReadOnlyList<KeeperCardInfo> KeeperCards;
    public static readonly IReadOnlyList<NewRuleCardInfo> NewRuleCards;

}