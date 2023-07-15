public static class NP_BlackBoardHelper
{
    public static void SetCurrentBlackBoardDataManager(NPBehaveGraph npBehaveGraph)
    {
        if (npBehaveGraph == null)
        {
            return;
        }
        NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager = npBehaveGraph.NpBlackBoardDataManager;
        NP_BlackBoardDataManager.BehaveId = npBehaveGraph.IdInConfig;
        NP_BlackBoardDataManager.IsSkill = npBehaveGraph is SkillGraph;
    }
}