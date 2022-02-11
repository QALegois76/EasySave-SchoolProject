using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace LibEasySave.TranslaterSystem
{
    public class Translater
    {
        #region private
        [JsonProperty]
        private ELangCode _activLang = ELangCode.EN;

        [JsonIgnore]
        private static Translater _instance = new Translater();

        [JsonProperty]
        private Dictionary<ELangCode, TranslatedText> _tralstedTexts = new Dictionary<ELangCode, TranslatedText>();
        #endregion


        #region accessor

        [JsonIgnore]
        public static Translater Instance => _instance;

        [JsonIgnore]
        public ITranslatedText TranslatedText => _tralstedTexts[_activLang];
        #endregion

        // constuctor
        private Translater() 
        {
        }

        public void Init()
        {
            var v = JsonConvert.DeserializeObject<Translater>(global::LibEasySave.Res.Resource1.LangData);
            Copy(v);
        }

        private void Copy(Translater src)
        {
            this._activLang = src._activLang;
            this._tralstedTexts = src._tralstedTexts;
        }


        public void SetActivLang(ELangCode code)
        {
            _activLang = code;
        }

        public string GetTextInfo(ESavingMode savingMode)
        {
            switch (savingMode)
            {
                default:
                case ESavingMode.FULL:
                    return TranslatedText.ESavingMode_FULL;

                case ESavingMode.DIFF:
                    return TranslatedText.ESavingMode_DIFF;
            }
        }

        public string GetTextInfo(ELangCode langCode)
        {
            switch (langCode)
            {
                default:
                case ELangCode.EN:
                    return TranslatedText.ELangCode_EN;

                case ELangCode.FR:
                    return TranslatedText.ELangCode_FR;
            }
        }



        public void TestSave()
        {
            _tralstedTexts.Clear();
            _tralstedTexts.Add(ELangCode.EN, new TranslatedText());
            _tralstedTexts.Add(ELangCode.FR, new TranslatedText());

            string str = JsonConvert.SerializeObject(this, Formatting.Indented);
            //using (StreamWriter strmW = new StreamWriter(@"C:\Users\qaleg\OneDrive\Documents\DOSSIER\SCHOOL\4 - CESI\INFORMATIQUE\Projet 2 - Programmation sytème\file.txt"))
            //{
            //    strmW.WriteLine(str);
            //    strmW.Close();
            //}

            string str2 = global::LibEasySave.Res.Resource1.LangData;

            try
            {
                var obj = JsonConvert.DeserializeObject<Translater>(str2);
                Copy(obj);
            }
            catch(Exception e)
            {

            }
            //global::LibEasySave.Res.Resource1.LangData = Encoding.UTF8.GetBytes(str);
        }

    }

    [Serializable]
    internal class TranslatedText : ITranslatedText
    {
        [JsonProperty]
        private string _eLangCode_EN;
        [JsonProperty]
        private string _eLangCode_FR;
        
        [JsonProperty]
        private string _eSavingMode_FULL;
        [JsonProperty]
        private string _eSavingMode_DIFF;

        [JsonProperty]
        private string _welcome;
        [JsonProperty]
        private string _bye;
        [JsonProperty]
        private string _disableText;
        [JsonProperty]
        public string _answerModelView;
        [JsonProperty]
        public string _failMsg;
        [JsonProperty]
        public string _sucessMsg;
        [JsonProperty]
        private string _errorMsg;
        [JsonProperty]
        private string _addTemplate;
        [JsonProperty]
        private string _removeTemplate;
        [JsonProperty]
        private string _renameTemplate;
        [JsonProperty]
        private string _editTemplate;
        [JsonProperty]
        private string _setRepSrcTemplate;
        [JsonProperty]
        private string _getRepSrcTemplate;
        [JsonProperty]
        private string _setRepDestTemplate;
        [JsonProperty]
        private string _getRepDestTemplate;
        [JsonProperty]
        private string _setRepSavingModeTemplate;
        [JsonProperty]
        private string _getRepSavingModeTemplate;
        [JsonProperty]
        private string _commandUnknow;
        //[JsonProperty]
        //private string _runAllTemplate;
        [JsonProperty]
        private string _runTemplate;
        [JsonProperty]
        private string _changeLangTemplate;
        [JsonProperty]
        private string _errorParameterWrongType;
        [JsonProperty]
        private string _errorParameterNull;
        [JsonProperty]
        private string _errorNameExistAlready;
        [JsonProperty]
        private string _errorNameDontExist;
        [JsonProperty]
        private string _errorMaxJob;
        [JsonProperty]
        private string _errorEditingJobNameNull;
        [JsonProperty]
        private string _errorEditingJobNull;
        [JsonProperty]
        private string _errorModelDontContainsEditingJob;
        [JsonProperty]
        private string _errorNoJobDeclared;
        [JsonProperty]
        private string _errorFolderDontExist;
        [JsonProperty]
        private string _errorNameNotAllowed;
        [JsonProperty]
        private string _errorCommandNotAvailable;



        [JsonIgnore]
        public string ELangCode_EN => _eLangCode_EN;
        [JsonIgnore]
        public string ELangCode_FR => _eLangCode_FR;

        [JsonIgnore]
        public string ESavingMode_FULL => _eSavingMode_FULL;
        [JsonIgnore]
        public string ESavingMode_DIFF => _eSavingMode_DIFF;


        [JsonIgnore]
        public string Welcome => _welcome;
        [JsonIgnore]
        public string Bye => _bye;
        [JsonIgnore]
        public string DisableText =>_disableText;
        [JsonIgnore]
        public string AnswerModelView => _answerModelView;
        [JsonIgnore]
        public string FailMsg => _failMsg;
        [JsonIgnore]
        public string SucessMsg => _sucessMsg;
        [JsonIgnore]
        public string ErrorMsg => _errorMsg;
        [JsonIgnore]
        public string AddTemplate => _addTemplate;
        [JsonIgnore]
        public string RemoveTemplate => _removeTemplate;
        [JsonIgnore]
        public string RenameTemplate => _renameTemplate;
        [JsonIgnore]
        public string EditTemplate => _editTemplate;
        [JsonIgnore]
        public string SetRepSrcTemplate => _setRepSrcTemplate;
        [JsonIgnore]
        public string GetRepSrcTemplate => _getRepSrcTemplate;
        [JsonIgnore]
        public string SetRepDestTemplate => _setRepDestTemplate;
        [JsonIgnore]
        public string GetRepDestTemplate => _getRepDestTemplate;
        [JsonIgnore]
        public string SetRepSavingModeTemplate => _setRepSavingModeTemplate;
        [JsonIgnore]
        public string GetRepSavingModeTemplate => _getRepSavingModeTemplate;
        [JsonIgnore]
        public string CommandUnknow => _commandUnknow;
        //[JsonIgnore]
        //public string RunAllTemplate => _runAllTemplate;
        [JsonIgnore]
        public string RunTemplate => _runTemplate;
        [JsonIgnore]
        public string ChangeLangTemplate => _changeLangTemplate;
        [JsonIgnore]
        public string ErrorParameterWrongType => _errorParameterWrongType;
        [JsonIgnore]
        public string ErrorParameterNull => _errorParameterNull;
        [JsonIgnore]
        public string ErrorNameExistAlready => _errorNameExistAlready;
        [JsonIgnore]
        public string ErrorNameDontExist => _errorNameDontExist;
        [JsonIgnore]
        public string ErrorMaxJob => _errorMaxJob;
        [JsonIgnore]
        public string ErrorEditingJobNameNull => _errorEditingJobNameNull;
        [JsonIgnore]
        public string ErrorEditingJobNull => _errorEditingJobNull;
        [JsonIgnore]
        public string ErrorModelDontContainsEditingJob => _errorModelDontContainsEditingJob;
        [JsonIgnore]
        public string ErrorNoJobDeclared => _errorNoJobDeclared;
        [JsonIgnore]
        public string ErrorFolderDontExist => _errorFolderDontExist;
        [JsonIgnore]
        public string ErrorNameNotAllowed => _errorNameNotAllowed;
        [JsonIgnore]
        public string ErrorCommandNotAvailable => _errorCommandNotAvailable;
    }

    public interface ITranslatedText
    {
        string ELangCode_EN { get; }
        string ELangCode_FR { get; }

        string ESavingMode_FULL { get; }
        string ESavingMode_DIFF { get; }

        string Welcome { get; }
        string Bye { get; }
        string DisableText { get; }
        string AnswerModelView { get; }
        string FailMsg { get; }
        string SucessMsg { get; }
        string ErrorMsg { get; }


        string AddTemplate { get; }
        string RemoveTemplate { get; }
        string RenameTemplate { get; }
        string EditTemplate { get; }

        string SetRepSrcTemplate { get; }
        string GetRepSrcTemplate { get; }
        string SetRepDestTemplate { get; }
        string GetRepDestTemplate { get; }
        string SetRepSavingModeTemplate { get; }
        string GetRepSavingModeTemplate { get; }

        string CommandUnknow { get; }

        //string RunAllTemplate { get; }
        string RunTemplate { get; }

        string ChangeLangTemplate { get; }


        string ErrorParameterWrongType { get; }
        string ErrorParameterNull { get; }
        string ErrorNameExistAlready { get; }
        string ErrorNameDontExist { get; }
        string ErrorMaxJob { get; }
        string ErrorEditingJobNameNull { get; }
        string ErrorEditingJobNull { get; }
        string ErrorModelDontContainsEditingJob { get; }
        string ErrorNoJobDeclared { get; }
        string ErrorFolderDontExist { get; }
        string ErrorNameNotAllowed { get; }

        string ErrorCommandNotAvailable { get; }


    }


    [Serializable]
    public enum ELangCode
    {
        EN,
        FR,
    }


}
