using Assets.Scripts.Creatures.Controllers;
using UnityEditor;

namespace Assets.Scripts.Creatures.Editors
{
    [CustomEditor(typeof(AIPetrolController))]
    internal class AIPetrolEditor : Editor
    {
        AIPetrolController conductionBase;
        private void OnSceneGUI()
        {
            conductionBase = (AIPetrolController)target;

            //AIActInfo[] actTrack = conductionBase.GetActTrack();
            //AIMoveInfo prevMoveInfo = null;
            //for (int i = 0; i < actTrack.Length; i++)
            //{
            //    if (actTrack[i].moveInfo != null && actTrack[i].moveInfo.spdMove != 0)
            //    {
            //        if (prevMoveInfo != null)
            //            DrawMoveTrack(prevMoveInfo, actTrack[i].GetMoveInfo());
            //        //else
            //        //    DrawMoveTrack(new AIMoveInfo(conductionBase.transform.localPosition.x, conductionBase.transform.localPosition.y, 1), actTrack[i].GetMoveInfo());
            //        prevMoveInfo = actTrack[i].GetMoveInfo();
            //    }
            //    if (actTrack[i].gazeInfo != null && actTrack[i].gazeInfo.secWait != 0)
            //    {
            //        if (prevMoveInfo != null)
            //            DrawGazeTrack(prevMoveInfo, actTrack[i].GetGazeInfo());
            //        else
            //            DrawGazeTrack(new AIMoveInfo(conductionBase.transform.localPosition.x, conductionBase.transform.localPosition.y, 1), actTrack[i].GetGazeInfo());
            //    }
            //}
        }

        /// <summary>
        /// 이동 트랙 그리기
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        //private void DrawMoveTrack(AIMoveInfo from, AIMoveInfo to)
        //{
        //    Handles.color = new Color(1, 1, 1, 0.5f);
        //    Handles.DrawLine(from.point(), to.point(), from.spdMove);
        //}

        //private void DrawGazeTrack(AIMoveInfo from, AIGazeInfo gaze)
        //{
        //    Handles.color = new Color(0, 1, 0, 0.5f);
        //    Handles.DrawWireArc(from.point(), Vector3.back, Vector3.up, 360, conductionBase.GetAIBaseController().GetDetectionPassiveController().range, gaze.secWait);
        //    Handles.color = new Color(1, 0, 0, 0.5f);
        //    Handles.DrawWireArc(from.point(), Vector3.forward, CalculationFunctions.DirFromAngle(gaze.degree), (conductionBase.GetAIBaseController().GetDetectionSightController().degree / 2), conductionBase.GetAIBaseController().GetDetectionSightController().range, gaze.secWait);
        //    Handles.DrawWireArc(from.point(), Vector3.forward, CalculationFunctions.DirFromAngle(gaze.degree), -(conductionBase.GetAIBaseController().GetDetectionSightController().degree / 2), conductionBase.GetAIBaseController().GetDetectionSightController().range, gaze.secWait);
        //}
    }
}
