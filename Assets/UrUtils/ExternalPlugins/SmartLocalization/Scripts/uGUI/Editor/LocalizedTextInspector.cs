
namespace SmartLocalization.Editor
{
    using UnityEditor;

    [CustomEditor(typeof(LocalizedText))]
    public class LocalizedTextInspector : Editor
    {
        private string selectedKey = null;

        void Awake()
        {
            LocalizedText textObject = ((LocalizedText)target);
            if (textObject != null)
            {
                selectedKey = textObject.localizedKey;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LocalizedText textObject = ((LocalizedText)target);
            selectedKey = LocalizedKeySelector.SelectKeyGUI(selectedKey, true, LocalizedObjectType.STRING);
            if (selectedKey != textObject.localizedKey)
            {
                Undo.RecordObject(textObject, "Set Smart Localization text");
                textObject.localizedKey = selectedKey;
            }
        }

    }
}