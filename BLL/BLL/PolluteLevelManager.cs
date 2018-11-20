using System;
using System.Data;
using System.Collections.Generic;
using MMShareBLL.DAL;
using MMShareBLL.Model;


namespace MMShareBLL.BLL 
{
    /// <summary>
    /// �ṩ����Ⱦ����Ĺ���
    /// </summary>
    public class PolluteLevelManager
    {
        private readonly PolluteLevelData dal = new PolluteLevelData();
        private readonly ParameterManager parameterManager = new ParameterManager();

        private const string CACHEKEY = "POLLUTELEVEL";
        private const string CAHCEKEY_LIVE = "POLLUTELEVEL_";


        #region ��̬
        private static PolluteLevelManager polluteLevelManager = new PolluteLevelManager();
        private static List<PolluteLevel> _allLevels = null;
        public static List<PolluteLevel> allLevels
        {
            get
            {
                if (null == _allLevels)
                {
                    _allLevels = new PolluteLevelManager().Query(true);
                }
                return _allLevels;
            }
        }
        
        #endregion
        /// <summary>
        /// ����ָ��APIֵ����ȡ����Ⱦ�ȼ���
        /// </summary>
        /// <param name="api">API</param>
        /// <param name="fromCache">�Ƿ�ӻ����ж�ȡ</param>
        /// <returns></returns>
        public PolluteLevel Query(float api, bool fromCache)
        {
            if (fromCache)
            {
                string levelKey = CAHCEKEY_LIVE + api;
                object val = null;
                if (CacheManager.Contains(levelKey))
                    val = CacheManager.GetData(levelKey);
                if (null == val)
                {
                    List<PolluteLevel> levelList = Query(true);
                    int maxLevelIndex = 0;

                    for (int i = 0; i < levelList.Count; i++)
                    {
                        PolluteLevel level = levelList[i];
                        if (level.EndRegion > maxLevelIndex)
                            maxLevelIndex = i;
                        if (level.StartRegion <= api && level.EndRegion >= api)
                            return level;
                    }
                    if (api > maxLevelIndex)
                    {
                        CacheManager.SetData(levelKey, levelList[maxLevelIndex].Clone());
                        return levelList[maxLevelIndex];
                    }
                    else
                        return null;
                }
                return (PolluteLevel)val;
            }
            else
                return dal.Query(api);
        }
        /// <summary>
        /// ��������Ũ�Ȼ�ȡ����Ⱦ�ȼ���
        /// </summary>
        /// <param name="paramID"></param>
        /// <param name="thinckness"></param>
        /// <param name="fromCache"></param>
        /// <returns></returns>
        public PolluteLevel QueryByThinckness(int paramID, float thinckness, bool fromCache)
        {
            if (thinckness < 0)
                return null;
            DataParameter param = parameterManager.GetParameter(paramID, fromCache);
            float api = DataCalculate.CalculateApi(param.ConcentrationLimits, thinckness);
            if (api < 0)
                return null;
            else
                return Query(api, fromCache);
        }

        public PolluteLevel QueryById(int levelId, bool fromCache)
        {

            List<PolluteLevel> levelList = Query(fromCache);

            for (int i = 0; i < levelList.Count; i++)
            {
                PolluteLevel level = levelList[i];
                if (levelId == level.LevelID)
                    return level;
            }
            return null;

        }

        /// <summary>
        /// �����Ⱦ��ȼ��б�
        /// </summary>
        /// <param name="fromCache">�Ƿ�ӻ����м��ش���</param>
        /// <returns></returns>
        public List<PolluteLevel> Query(bool fromCache)
        {
            List<PolluteLevel> level = null;
            if (fromCache && CacheManager.Contains(CACHEKEY))
                level = CacheManager.GetData(CACHEKEY) as List<PolluteLevel>;
            if (level == null)
            {
                level = dal.Query();
                if (fromCache)
                    CacheManager.SetData(CACHEKEY, level);
            }
            return level;
        }

    }
}



