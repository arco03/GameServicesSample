﻿using System.IO;
using UnityEditor;
using UnityEngine;

namespace TheraBytes.BetterUi.Editor
{
    public class AssemblyDefinitionsPage : WizardPage
    {
        static readonly string[] ASMDEF_FILES =
            { 
                "Plugins/TheraBytes/BetterUI/Runtime/TheraBytes.BetterUi.Runtime.asmdef",
                "Plugins/TheraBytes/BetterUI/Editor/TheraBytes.BetterUi.Editor.asmdef",
                "Plugins/TheraBytes/BetterUI_TextMeshPro/Runtime/TheraBytes.BetterUi.asmref",
                "Plugins/TheraBytes/BetterUI_TextMeshPro/Editor/TheraBytes.BetterUi.Editor.asmref",
            };

        const string TEXTMESH_PRO_PATH = "Plugins/TheraBytes/BetterUI_TextMeshPro";

        const string PACKAGE_PATH_INCL_TMP = "Plugins/TheraBytes/BetterUI/packages/asmdef_incl_TextMeshPro.unitypackage";
        const string PACKAGE_PATH_EXCL_TMP = "Plugins/TheraBytes/BetterUI/packages/asmdef_excl_TextMeshPro.unitypackage";

        const string PERSISTENT_KEY = "asmdef";

        public override string NameId { get { return "AssemblyDefinitionsPage"; } }

        public AssemblyDefinitionsPage(IWizard wizard)
            : base(wizard)
        {
        }

        protected override void OnInitialize()
        {
            Add(new InfoWizardPageElement("Assembly Definition Files installation", InfoType.Header));

            // TextMesh Pro
            Add(new SeparatorWizardPageElement());
            Add(new InfoWizardPageElement("You can install Assembly Definition Files for Better UI here. " +
                "If you don't know what this is, you most likely don't want to install it."));
            Add(new SeparatorWizardPageElement());

            string mainAsmdefFilePath = Path.Combine(Application.dataPath, ASMDEF_FILES[0]);
            if (File.Exists(mainAsmdefFilePath))
            {
                // DELETE

                Add(new InfoWizardPageElement("Assembly Definition Files are already installed."));

                Add(new ValueWizardPageElement<string>(PERSISTENT_KEY,
                    (o, v) =>
                    {
                        if(GUILayout.Button("Remove Assembly Definition Files"))
                        {
                            wizard.DoReloadOperation(this, () =>
                            {
                                foreach (string subPath in ASMDEF_FILES)
                                {
                                    string filePath = Path.Combine(Application.dataPath, subPath);
                                    if (File.Exists(filePath))
                                    {
                                        File.Delete(filePath);
                                    }
                                }

                                AssetDatabase.Refresh();
                                v = null;
                            });
                        }

                        return v;
                    }).MarkComplete());
            }
            else
            {
                // INSTALL

                Add(new ValueWizardPageElement<string>(PERSISTENT_KEY,
                    (o, v) =>
                    {
                        if (GUILayout.Button("Install AssemblyDefinitions"))
                        {
                            string tmpPath = Path.Combine(Application.dataPath, TEXTMESH_PRO_PATH);
                            string packageName = (Directory.Exists(tmpPath))
                                ? PACKAGE_PATH_INCL_TMP
                                : PACKAGE_PATH_EXCL_TMP;

                            wizard.DoReloadOperation(this, () =>
                            {
                                AssetDatabase.ImportPackage(Path.Combine(Application.dataPath, packageName), false);
                                v = packageName;
                            });
                        }

                        return v;
                    }).MarkComplete());
            }
        }
    }
}
