using Michsky.UI.Reach;
using Physiqia.PTable;
using TMPro;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

namespace Physiqia.TheLab
{
    public class PeriodicTableController : MonoBehaviour
    {
        public byte viewMode;
        public GameObject tablesCanvas;
        public GameObject elementsTable;
        public GameObject particlesTable;
        public GameObject atomCanvas;
        public ButtonManager ElementsTableButton;
        public ButtonManager ParticlesTableButton;

        /// <summary>
        /// 1
        /// </summary>
        public CanvasGroup[] groupAlkaliMetals;

        /// <summary>
        /// 2
        /// </summary>
        public CanvasGroup[] groupAlkalineEarthMetals;

        /// <summary>
        /// 3
        /// </summary>
        public CanvasGroup[] groupLanthanoids;

        /// <summary>
        /// 4
        /// </summary>
        public CanvasGroup[] groupActinoids;

        /// <summary>
        /// 5
        /// </summary>
        public CanvasGroup[] groupTransitionMetals;

        /// <summary>
        /// 6
        /// </summary>
        public CanvasGroup[] groupPoorMetals;

        /// <summary>
        /// 7
        /// </summary>
        public CanvasGroup[] groupMetalloids;

        /// <summary>
        /// 8
        /// </summary>
        public CanvasGroup[] groupNonmetals;

        /// <summary>
        /// 9
        /// </summary>
        public CanvasGroup[] groupHalogens;

        /// <summary>
        /// 10
        /// </summary>
        public CanvasGroup[] groupNobleGas;

        public GameObject atomInfoPanel;
        public GameObject particleInfoPanel;

        public string currentAtom;
        public IPTABLEDATA_Element currentAtomData;

        public TMP_Text atomInfo_Title;
        public TMP_Text atomView_Title;

        private PTABLEDATA_Factory pTABLEDATA_Factory;
        public ButtonManager atomViewClassicButton;
        public ButtonManager atomViewQuanticButton;

        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            viewMode = 1;
            pTABLEDATA_Factory = new PTABLEDATA_Factory();
            _hideCanvas();
            if (viewMode == 1)
            {
                DisplayPeriodicTable();
                ShowElementsTable();
            }
            else
            {
                DisplayAtom();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void Update() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="atom"></param>
        public void SetAtom(string atom)
        {
            currentAtom = atom;
            LoadAtomData();
            atomInfoPanel.SetActive(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public void ViewParticle(string p)
        {
            LoadParticleData();
            particleInfoPanel.SetActive(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowElementsTable()
        {
            elementsTable.SetActive(true);
            particlesTable.SetActive(false);

            ElementsTableButton.HighlightButton();
            ElementsTableButton.Interactable(false);

            ParticlesTableButton.UnhihglightButton();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowParticlesTable()
        {
            elementsTable.SetActive(false);
            particlesTable.SetActive(true);

            ParticlesTableButton.HighlightButton();
            ParticlesTableButton.Interactable(false);

            ElementsTableButton.UnhihglightButton();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CloseAtomInfoPanel()
        {
            atomInfoPanel.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisplayAtom()
        {
            ActivateAtomCanvas();
            ConfigureAtomView();
            GenerateAtomFromData();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ActivateAtomCanvas()
        {
            atomCanvas.SetActive(true);
            tablesCanvas.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConfigureAtomView()
        {
            atomViewClassicButton.HighlightButton();
        }

        /// <summary>
        /// 
        /// </summary>
        private void GenerateAtomFromData()
        {
            var atomGenerator = GetComponent<AtomGenerator>();
            atomGenerator.atomicMass = float.Parse(currentAtomData.AtomicMass);
            atomGenerator.atomicNumber = int.Parse(currentAtomData.AtomicNumber);
            atomGenerator.electronicConfiguration = currentAtomData.ElectronicConfiguration;
            atomGenerator.GenerateAtom();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DisplayPeriodicTable()
        {
            tablesCanvas.SetActive(true);
            atomCanvas.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void CloseAtomView()
        {
            this.GetComponent<AtomGenerator>().ClearAtom();
            DisplayPeriodicTable();
            atomViewClassicButton.Interactable(true);
            AtomOrbitCamera atomOrbitCamera = GetComponent<AtomOrbitCamera>();
            atomOrbitCamera.SetTarget(null);
        }

        /// <summary>
        /// 
        /// </summary>
        public void HighlighAll()
        {
            CanvasGroup[][] allGroups = new CanvasGroup[][]
            {
                groupAlkaliMetals,
                groupAlkalineEarthMetals,
                groupLanthanoids,
                groupActinoids,
                groupTransitionMetals,
                groupPoorMetals,
                groupMetalloids,
                groupNonmetals,
                groupHalogens,
                groupNobleGas
            };

            foreach (var groupArray in allGroups)
            {
                foreach (CanvasGroup group in groupArray)
                {
                    group.alpha = 1f;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        public void HighlighGroup(int idx)
        {
            HideGroups();

            CanvasGroup[] targetGroup = idx switch
            {
                1 => groupAlkaliMetals,
                2 => groupAlkalineEarthMetals,
                3 => groupLanthanoids,
                4 => groupActinoids,
                5 => groupTransitionMetals,
                6 => groupPoorMetals,
                7 => groupMetalloids,
                8 => groupNonmetals,
                9 => groupHalogens,
                10 => groupNobleGas,
                _ => null
            };

            if (targetGroup != null)
            {
                foreach (CanvasGroup group in targetGroup)
                {
                    group.alpha = 1f;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void HideGroups()
        {
            CanvasGroup[][] allGroups = new CanvasGroup[][]
            {
                groupAlkaliMetals,
                groupAlkalineEarthMetals,
                groupLanthanoids,
                groupActinoids,
                groupTransitionMetals,
                groupPoorMetals,
                groupMetalloids,
                groupNonmetals,
                groupHalogens,
                groupNobleGas
            };

            foreach (var groupArray in allGroups)
            {
                foreach (CanvasGroup group in groupArray)
                {
                    group.alpha = 0.3f;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _hideCanvas()
        {
            tablesCanvas.SetActive(false);
            atomCanvas.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadAtomData()
        {
            string className = getPTableElementClass(currentAtom);
            IPTABLEDATA_Element ptableElement = pTABLEDATA_Factory.GetElementBySymbol(className);

            if (ptableElement != null)
            {
                this.currentAtomData = ptableElement;
            }
            atomInfo_Title.text = ptableElement.Name;
            atomView_Title.text = ptableElement.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadParticleData()
        {
            // TODO: LOAD PARTICLE DATA
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private string getPTableElementClass(string symbol)
        {
            return "Physiqia.TheLab.PTable.PTABLEDATA_" + symbol.ToUpper() + "_L" + LangEnum.GetEnumString(LangEnum.English).ToUpper();
        }
    }

}
