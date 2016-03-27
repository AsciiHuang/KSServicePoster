using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KSServicePoster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String ksData = "";

        private Constants.LayoutType layoutType = Constants.LayoutType.Landscape_Full;

        private List<MediaData> tLItems = new List<MediaData>();
        private List<MediaData> tCItems = new List<MediaData>();
        private List<MediaData> tRItems = new List<MediaData>();

        private List<MediaData> cLItems = new List<MediaData>();
        private List<MediaData> cCItems = new List<MediaData>();
        private List<MediaData> cRItems = new List<MediaData>();

        private List<MediaData> bLItems = new List<MediaData>();
        private List<MediaData> bCItems = new List<MediaData>();
        private List<MediaData> bRItems = new List<MediaData>();

        private Marquee marqueeItem = new Marquee();
        private Storyboard marqueeStoryboard = new Storyboard();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            String path = System.AppDomain.CurrentDomain.BaseDirectory;
            String fullFileName = System.AppDomain.CurrentDomain.FriendlyName;
            int dotIndex = fullFileName.IndexOf('.');
            String fileName = fullFileName.Substring(0, dotIndex);
            String ksdFileName = String.Format("{0}{1}.ksd", path, fileName);

            if (File.Exists(ksdFileName))
            {
                String fileContent = File.ReadAllText(ksdFileName, Encoding.Unicode);
                byte[] encContentBytes = Convert.FromBase64String(fileContent);
                RC4 rc4Obj = new RC4("a555ab555bc555cd555d");
                rc4Obj.EncryptInPlace(encContentBytes);
                ksData = getString(encContentBytes);
                if (String.IsNullOrEmpty(ksData) || ksData.Length < 492)
                {
                    MessageBox.Show(String.Format("{0}.ksd 檔案格式錯誤", fileName));
                    this.Close();
                }
                else
                {
                    initData(fileName);
                    if (getCorractLayout() == null)
                    {
                        MessageBox.Show(String.Format("{0}.ksd 檔案格式錯誤", fileName));
                        this.Close();
                    }
                    else
                    {
                        showCorractLayout();

                        MediaPanel tLPanel = getCorrectTLPanel();
                        if (tLPanel != null && tLItems.Count > 0)
                        {
                            tLPanel.setData(tLItems);
                        }

                        MediaPanel tCPanel = getCorrectTCPanel();
                        if (tCPanel != null && tCItems.Count > 0)
                        {
                            tCPanel.setData(tCItems);
                        }

                        MediaPanel tRPanel = getCorrectTRPanel();
                        if (tRPanel != null && tRItems.Count > 0)
                        {
                            tRPanel.setData(tRItems);
                        }

                        MediaPanel cLPanel = getCorrectCLPanel();
                        if (cLPanel != null && cLItems.Count > 0)
                        {
                            cLPanel.setData(cLItems);
                        }

                        MediaPanel cCPanel = getCorrectCCPanel();
                        if (cCPanel != null && cCItems.Count > 0)
                        {
                            cCPanel.setData(cCItems);
                        }

                        MediaPanel cRPanel = getCorrectCRPanel();
                        if (cRPanel != null && cRItems.Count > 0)
                        {
                            cRPanel.setData(cRItems);
                        }

                        MediaPanel bLPanel = getCorrectBLPanel();
                        if (bLPanel != null && bLItems.Count > 0)
                        {
                            bLPanel.setData(bLItems);
                        }

                        MediaPanel bCPanel = getCorrectBCPanel();
                        if (bCPanel != null && bCItems.Count > 0)
                        {
                            bCPanel.setData(bCItems);
                        }

                        MediaPanel bRPanel = getCorrectBRPanel();
                        if (bRPanel != null && bRItems.Count > 0)
                        {
                            bRPanel.setData(bRItems);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(String.Format("同層資料夾中找不到 {0}.ksd 檔案", fileName));
                this.Close();
            }
        }

        private void initData(String fileName)
        {
            JObject jRoot = null;
            try
            {
                jRoot = JObject.Parse(ksData);
                if (jRoot != null && jRoot.HasValues)
                {
                    String typeValue = jRoot["Type"].ToString();
                    layoutType = (Constants.LayoutType)Enum.Parse(typeof(Constants.LayoutType), typeValue);

                    fillMediaList(tLItems, (JArray)jRoot["TL"]);
                    fillMediaList(tCItems, (JArray)jRoot["TC"]);
                    fillMediaList(tRItems, (JArray)jRoot["TR"]);

                    fillMediaList(cLItems, (JArray)jRoot["CL"]);
                    fillMediaList(cCItems, (JArray)jRoot["CC"]);
                    fillMediaList(cRItems, (JArray)jRoot["CR"]);

                    fillMediaList(bLItems, (JArray)jRoot["BL"]);
                    fillMediaList(bCItems, (JArray)jRoot["BC"]);
                    fillMediaList(bRItems, (JArray)jRoot["BR"]);

                    marqueeItem.Content = jRoot["Marquee"]["Text"].ToString();
                    marqueeItem.Background = jRoot["Marquee"]["Background"].ToString();
                    marqueeItem.FontColor = jRoot["Marquee"]["FontColor"].ToString();

                    updateMarqueeData();
                }
                else
                {
                    MessageBox.Show(String.Format("{0}.ksd 檔案格式錯誤", fileName));
                    this.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("{0}.ksd 檔案格式錯誤", fileName));
                this.Close();
            }
        }

        private void updateMarqueeData()
        {
            if (!String.IsNullOrEmpty(marqueeItem.Content))
            {
                TextBlock textMarquee = null;
                Canvas canvasMarquee = null;
                if (layoutType == Constants.LayoutType.Landscape_Full_BottomMarquee)
                {
                    textMarquee = marquee_Landscape_Full_BottomMarquee;
                    canvasMarquee = marquee_background_Landscape_Full_BottomMarquee;
                }
                else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R11)
                {
                    textMarquee = marquee_Landscape_BottomMarquee_L1_R11;
                    canvasMarquee = marquee_background_Landscape_BottomMarquee_L1_R11;
                }
                else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R21)
                {
                    textMarquee = marquee_Landscape_BottomMarquee_L1_R21;
                    canvasMarquee = marquee_background_Landscape_BottomMarquee_L1_R21;
                }
                else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_C1_R1)
                {
                    textMarquee = marquee_Landscape_BottomMarquee_L1_C1_R1;
                    canvasMarquee = marquee_background_Landscape_BottomMarquee_L1_C1_R1;
                }
                else if (layoutType == Constants.LayoutType.Portrait_Full_TopMarquee)
                {
                    textMarquee = marquee_Portrait_Full_TopMarquee;
                    canvasMarquee = marquee_background_Portrait_Full_TopMarquee;
                }
                else if (layoutType == Constants.LayoutType.Portrait_CenterMarquee_T1_B3)
                {
                    textMarquee = marquee_Portrait_CenterMarquee_T1_B3;
                    canvasMarquee = marquee_background_Portrait_CenterMarquee_T1_B3;
                }

                if (textMarquee != null)
                {
                    textMarquee.Text = marqueeItem.Content;
                    Color fontColor = (Color)ColorConverter.ConvertFromString(marqueeItem.FontColor);
                    Color backgroundColor = (Color)ColorConverter.ConvertFromString(marqueeItem.Background);
                    textMarquee.Foreground = new SolidColorBrush(fontColor);
                    canvasMarquee.Background = new SolidColorBrush(backgroundColor);
                    DoubleAnimation doubleAnimation = new DoubleAnimation();
                    doubleAnimation.From = this.ActualWidth;
                    //doubleAnimation.To = -textMarquee.ActualWidth;
                    doubleAnimation.To = -marqueeItem.Content.Length * 50;
                    doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                    doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(20));
                    textMarquee.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
                }
            }
        }

        private void fillMediaList(List<MediaData> itmes, JArray jItems)
        {
            for (int i = 0; i < jItems.Count; ++i)
            {
                MediaData newData = new MediaData();
                JToken jItemToken = jItems[i];
                JObject jItemObj = (JObject)jItemToken;
                String typeValue = jItemObj["Type"].ToString();
                String durationValue = jItemObj["Duration"].ToString();

                newData.Type = (Constants.MediaType)Enum.Parse(typeof(Constants.MediaType), typeValue);
                newData.Duration = (Constants.MediaDuration)Enum.Parse(typeof(Constants.MediaDuration), durationValue);
                newData.Path = jItemObj["Path"].ToString();
                newData.InternalPath = jItemObj["InternalPath"].ToString();

                itmes.Add(newData);
            }

        }

        private void showCorractLayout()
        {
            Landscape_Full.Visibility = System.Windows.Visibility.Hidden;
            Landscape_L1_R21.Visibility = System.Windows.Visibility.Hidden;
            Landscape_L1_R111.Visibility = System.Windows.Visibility.Hidden;
            Landscape_Full_BottomMarquee.Visibility = System.Windows.Visibility.Hidden;
            Landscape_BottomMarquee_L1_R11.Visibility = System.Windows.Visibility.Hidden;
            Landscape_BottomMarquee_L1_R21.Visibility = System.Windows.Visibility.Hidden;
            Landscape_L1_C1_R1.Visibility = System.Windows.Visibility.Hidden;
            Landscape_BottomMarquee_L1_C1_R1.Visibility = System.Windows.Visibility.Hidden;
            Portrait_Full.Visibility = System.Windows.Visibility.Hidden;
            Portrait_Full_TopMarquee.Visibility = System.Windows.Visibility.Hidden;
            Portrait_CenterMarquee_T1_B3.Visibility = System.Windows.Visibility.Hidden;
            Portrait_T1_C1_B1.Visibility = System.Windows.Visibility.Hidden;
            Portrait_T1_C1_CL1_CR1_B1.Visibility = System.Windows.Visibility.Hidden;

            Grid correctLayout = getCorractLayout();
            if (correctLayout != null)
            {
                correctLayout.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private Grid getCorractLayout()
        {
            Grid resGrid = null;

            if (layoutType == Constants.LayoutType.Landscape_Full)
            {
                resGrid = Landscape_Full;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R21)
            {
                resGrid = Landscape_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R111)
            {
                resGrid = Landscape_L1_R111;
            }
            else if (layoutType == Constants.LayoutType.Landscape_Full_BottomMarquee)
            {
                resGrid = Landscape_Full_BottomMarquee;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R11)
            {
                resGrid = Landscape_BottomMarquee_L1_R11;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R21)
            {
                resGrid = Landscape_BottomMarquee_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_C1_R1)
            {
                resGrid = Landscape_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_C1_R1)
            {
                resGrid = Landscape_BottomMarquee_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full)
            {
                resGrid = Portrait_Full;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full_TopMarquee)
            {
                resGrid = Portrait_Full_TopMarquee;
            }
            else if (layoutType == Constants.LayoutType.Portrait_CenterMarquee_T1_B3)
            {
                resGrid = Portrait_CenterMarquee_T1_B3;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_B1)
            {
                resGrid = Portrait_T1_C1_B1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_CL1_CR1_B1)
            {
                resGrid = Portrait_T1_C1_CL1_CR1_B1;
            }

            return resGrid;
        }

        private MediaPanel getCorrectTLPanel()
        {
            MediaPanel resPanel = null;

            if (layoutType == Constants.LayoutType.Landscape_Full)
            {
                //resPanel = mediaPanel_TL_Landscape_Full;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R21)
            {
                //resPanel = mediaPanel_TL_Landscape_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R111)
            {
                //resPanel = mediaPanel_TL_Landscape_L1_R111;
            }
            else if (layoutType == Constants.LayoutType.Landscape_Full_BottomMarquee)
            {
                //resPanel = mediaPanel_TL_Landscape_Full_BottomMarquee;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R11)
            {
                //resPanel = mediaPanel_TL_Landscape_BottomMarquee_L1_R11;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R21)
            {
                //resPanel = mediaPanel_TL_Landscape_BottomMarquee_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_C1_R1)
            {
                //resPanel = mediaPanel_TL_Landscape_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_C1_R1)
            {
                //resPanel = mediaPanel_TL_Landscape_BottomMarquee_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full)
            {
                //resPanel = mediaPanel_TL_Portrait_Full;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full_TopMarquee)
            {
                //resPanel = mediaPanel_TL_Portrait_Full_TopMarquee;
            }
            else if (layoutType == Constants.LayoutType.Portrait_CenterMarquee_T1_B3)
            {
                //resPanel = mediaPanel_TL_Portrait_CenterMarquee_T1_B3;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_B1)
            {
                //resPanel = mediaPanel_TL_Portrait_T1_C1_B1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_CL1_CR1_B1)
            {
                //resPanel = mediaPanel_TL_Portrait_T1_C1_CL1_CR1_B1;
            }

            return resPanel;
        }

        private MediaPanel getCorrectTCPanel()
        {
            MediaPanel resPanel = null;

            if (layoutType == Constants.LayoutType.Landscape_Full)
            {
                //resPanel = mediaPanel_TC_Landscape_Full;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R21)
            {
                //resPanel = mediaPanel_TC_Landscape_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R111)
            {
                //resPanel = mediaPanel_TC_Landscape_L1_R111;
            }
            else if (layoutType == Constants.LayoutType.Landscape_Full_BottomMarquee)
            {
                //resPanel = mediaPanel_TC_Landscape_Full_BottomMarquee;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R11)
            {
                //resPanel = mediaPanel_TC_Landscape_BottomMarquee_L1_R11;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R21)
            {
                //resPanel = mediaPanel_TC_Landscape_BottomMarquee_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_C1_R1)
            {
                //resPanel = mediaPanel_TC_Landscape_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_C1_R1)
            {
                //resPanel = mediaPanel_TC_Landscape_BottomMarquee_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full)
            {
                //resPanel = mediaPanel_TC_Portrait_Full;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full_TopMarquee)
            {
                //resPanel = mediaPanel_TC_Portrait_Full_TopMarquee;
            }
            else if (layoutType == Constants.LayoutType.Portrait_CenterMarquee_T1_B3)
            {
                resPanel = mediaPanel_TC_Portrait_CenterMarquee_T1_B3;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_B1)
            {
                resPanel = mediaPanel_TC_Portrait_T1_C1_B1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_CL1_CR1_B1)
            {
                resPanel = mediaPanel_TC_Portrait_T1_C1_CL1_CR1_B1;
            }

            return resPanel;
        }

        private MediaPanel getCorrectTRPanel()
        {
            MediaPanel resPanel = null;

            if (layoutType == Constants.LayoutType.Landscape_Full)
            {
                //resPanel = mediaPanel_TR_Landscape_Full;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R21)
            {
                resPanel = mediaPanel_TR_Landscape_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R111)
            {
                resPanel = mediaPanel_TR_Landscape_L1_R111;
            }
            else if (layoutType == Constants.LayoutType.Landscape_Full_BottomMarquee)
            {
                //resPanel = mediaPanel_TR_Landscape_Full_BottomMarquee;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R11)
            {
                resPanel = mediaPanel_TR_Landscape_BottomMarquee_L1_R11;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R21)
            {
                resPanel = mediaPanel_TR_Landscape_BottomMarquee_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_C1_R1)
            {
                //resPanel = mediaPanel_TR_Landscape_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_C1_R1)
            {
                //resPanel = mediaPanel_TR_Landscape_BottomMarquee_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full)
            {
                //resPanel = mediaPanel_TR_Portrait_Full;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full_TopMarquee)
            {
                //resPanel = mediaPanel_TR_Portrait_Full_TopMarquee;
            }
            else if (layoutType == Constants.LayoutType.Portrait_CenterMarquee_T1_B3)
            {
                //resPanel = mediaPanel_TR_Portrait_CenterMarquee_T1_B3;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_B1)
            {
                //resPanel = mediaPanel_TR_Portrait_T1_C1_B1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_CL1_CR1_B1)
            {
                //resPanel = mediaPanel_TR_Portrait_T1_C1_CL1_CR1_B1;
            }

            return resPanel;
        }

        private MediaPanel getCorrectCLPanel()
        {
            MediaPanel resPanel = null;

            if (layoutType == Constants.LayoutType.Landscape_Full)
            {
                //resPanel = mediaPanel_CL_Landscape_Full;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R21)
            {
                resPanel = mediaPanel_CL_Landscape_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R111)
            {
                resPanel = mediaPanel_CL_Landscape_L1_R111;
            }
            else if (layoutType == Constants.LayoutType.Landscape_Full_BottomMarquee)
            {
                //resPanel = mediaPanel_CL_Landscape_Full_BottomMarquee;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R11)
            {
                resPanel = mediaPanel_CL_Landscape_BottomMarquee_L1_R11;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R21)
            {
                //resPanel = mediaPanel_CL_Landscape_BottomMarquee_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_C1_R1)
            {
                resPanel = mediaPanel_CL_Landscape_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_C1_R1)
            {
                resPanel = mediaPanel_CL_Landscape_BottomMarquee_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full)
            {
                //resPanel = mediaPanel_CL_Portrait_Full;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full_TopMarquee)
            {
                //resPanel = mediaPanel_CL_Portrait_Full_TopMarquee;
            }
            else if (layoutType == Constants.LayoutType.Portrait_CenterMarquee_T1_B3)
            {
                //resPanel = mediaPanel_CL_Portrait_CenterMarquee_T1_B3;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_B1)
            {
                //resPanel = mediaPanel_CL_Portrait_T1_C1_B1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_CL1_CR1_B1)
            {
                resPanel = mediaPanel_CL_Portrait_T1_C1_CL1_CR1_B1;
            }

            return resPanel;
        }

        private MediaPanel getCorrectCCPanel()
        {
            MediaPanel resPanel = null;

            if (layoutType == Constants.LayoutType.Landscape_Full)
            {
                resPanel = mediaPanel_CC_Landscape_Full;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R21)
            {
                //resPanel = mediaPanel_CC_Landscape_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R111)
            {
                //resPanel = mediaPanel_CC_Landscape_L1_R111;
            }
            else if (layoutType == Constants.LayoutType.Landscape_Full_BottomMarquee)
            {
                resPanel = mediaPanel_CC_Landscape_Full_BottomMarquee;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R11)
            {
                //resPanel = mediaPanel_CC_Landscape_BottomMarquee_L1_R11;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R21)
            {
                resPanel = mediaPanel_CC_Landscape_BottomMarquee_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_C1_R1)
            {
                resPanel = mediaPanel_CC_Landscape_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_C1_R1)
            {
                resPanel = mediaPanel_CC_Landscape_BottomMarquee_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full)
            {
                resPanel = mediaPanel_CC_Portrait_Full;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full_TopMarquee)
            {
                resPanel = mediaPanel_CC_Portrait_Full_TopMarquee;
            }
            else if (layoutType == Constants.LayoutType.Portrait_CenterMarquee_T1_B3)
            {
                //resPanel = mediaPanel_CC_Portrait_CenterMarquee_T1_B3;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_B1)
            {
                resPanel = mediaPanel_CC_Portrait_T1_C1_B1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_CL1_CR1_B1)
            {
                //resPanel = mediaPanel_CC_Portrait_T1_C1_CL1_CR1_B1;
            }

            return resPanel;
        }

        private MediaPanel getCorrectCRPanel()
        {
            MediaPanel resPanel = null;

            if (layoutType == Constants.LayoutType.Landscape_Full)
            {
                //resPanel = mediaPanel_CR_Landscape_Full;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R21)
            {
                //resPanel = mediaPanel_CR_Landscape_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R111)
            {
                resPanel = mediaPanel_CR_Landscape_L1_R111;
            }
            else if (layoutType == Constants.LayoutType.Landscape_Full_BottomMarquee)
            {
                //resPanel = mediaPanel_CR_Landscape_Full_BottomMarquee;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R11)
            {
                //resPanel = mediaPanel_CR_Landscape_BottomMarquee_L1_R11;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R21)
            {
                //resPanel = mediaPanel_CR_Landscape_BottomMarquee_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_C1_R1)
            {
                resPanel = mediaPanel_CR_Landscape_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_C1_R1)
            {
                resPanel = mediaPanel_CR_Landscape_BottomMarquee_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full)
            {
                //resPanel = mediaPanel_CR_Portrait_Full;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full_TopMarquee)
            {
                //resPanel = mediaPanel_CR_Portrait_Full_TopMarquee;
            }
            else if (layoutType == Constants.LayoutType.Portrait_CenterMarquee_T1_B3)
            {
                //resPanel = mediaPanel_CR_Portrait_CenterMarquee_T1_B3;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_B1)
            {
                //resPanel = mediaPanel_CR_Portrait_T1_C1_B1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_CL1_CR1_B1)
            {
                resPanel = mediaPanel_CR_Portrait_T1_C1_CL1_CR1_B1;
            }

            return resPanel;
        }

        private MediaPanel getCorrectBLPanel()
        {
            MediaPanel resPanel = null;

            if (layoutType == Constants.LayoutType.Landscape_Full)
            {
                //resPanel = mediaPanel_BL_Landscape_Full;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R21)
            {
                //resPanel = mediaPanel_BL_Landscape_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R111)
            {
                //resPanel = mediaPanel_BL_Landscape_L1_R111;
            }
            else if (layoutType == Constants.LayoutType.Landscape_Full_BottomMarquee)
            {
                //resPanel = mediaPanel_BL_Landscape_Full_BottomMarquee;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R11)
            {
                //resPanel = mediaPanel_BL_Landscape_BottomMarquee_L1_R11;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R21)
            {
                //resPanel = mediaPanel_BL_Landscape_BottomMarquee_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_C1_R1)
            {
                //resPanel = mediaPanel_BL_Landscape_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_C1_R1)
            {
                //resPanel = mediaPanel_BL_Landscape_BottomMarquee_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full)
            {
                //resPanel = mediaPanel_BL_Portrait_Full;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full_TopMarquee)
            {
                //resPanel = mediaPanel_BL_Portrait_Full_TopMarquee;
            }
            else if (layoutType == Constants.LayoutType.Portrait_CenterMarquee_T1_B3)
            {
                //resPanel = mediaPanel_BL_Portrait_CenterMarquee_T1_B3;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_B1)
            {
                //resPanel = mediaPanel_BL_Portrait_T1_C1_B1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_CL1_CR1_B1)
            {
                //resPanel = mediaPanel_BL_Portrait_T1_C1_CL1_CR1_B1;
            }

            return resPanel;
        }

        private MediaPanel getCorrectBCPanel()
        {
            MediaPanel resPanel = null;

            if (layoutType == Constants.LayoutType.Landscape_Full)
            {
                //resPanel = mediaPanel_BC_Landscape_Full;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R21)
            {
                //resPanel = mediaPanel_BC_Landscape_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R111)
            {
                //resPanel = mediaPanel_BC_Landscape_L1_R111;
            }
            else if (layoutType == Constants.LayoutType.Landscape_Full_BottomMarquee)
            {
                //resPanel = mediaPanel_BC_Landscape_Full_BottomMarquee;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R11)
            {
                //resPanel = mediaPanel_BC_Landscape_BottomMarquee_L1_R11;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R21)
            {
                //resPanel = mediaPanel_BC_Landscape_BottomMarquee_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_C1_R1)
            {
                //resPanel = mediaPanel_BC_Landscape_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_C1_R1)
            {
                //resPanel = mediaPanel_BC_Landscape_BottomMarquee_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full)
            {
                //resPanel = mediaPanel_BC_Portrait_Full;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full_TopMarquee)
            {
                //resPanel = mediaPanel_BC_Portrait_Full_TopMarquee;
            }
            else if (layoutType == Constants.LayoutType.Portrait_CenterMarquee_T1_B3)
            {
                resPanel = mediaPanel_BC_Portrait_CenterMarquee_T1_B3;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_B1)
            {
                resPanel = mediaPanel_BC_Portrait_T1_C1_B1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_CL1_CR1_B1)
            {
                resPanel = mediaPanel_BC_Portrait_T1_C1_CL1_CR1_B1;
            }

            return resPanel;
        }

        private MediaPanel getCorrectBRPanel()
        {
            MediaPanel resPanel = null;

            if (layoutType == Constants.LayoutType.Landscape_Full)
            {
                //resPanel = mediaPanel_BR_Landscape_Full;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R21)
            {
                resPanel = mediaPanel_BR_Landscape_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_R111)
            {
                resPanel = mediaPanel_BR_Landscape_L1_R111;
            }
            else if (layoutType == Constants.LayoutType.Landscape_Full_BottomMarquee)
            {
                //resPanel = mediaPanel_BR_Landscape_Full_BottomMarquee;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R11)
            {
                resPanel = mediaPanel_BR_Landscape_BottomMarquee_L1_R11;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_R21)
            {
                resPanel = mediaPanel_BR_Landscape_BottomMarquee_L1_R21;
            }
            else if (layoutType == Constants.LayoutType.Landscape_L1_C1_R1)
            {
                //resPanel = mediaPanel_BR_Landscape_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Landscape_BottomMarquee_L1_C1_R1)
            {
                //resPanel = mediaPanel_BR_Landscape_BottomMarquee_L1_C1_R1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full)
            {
                //resPanel = mediaPanel_BR_Portrait_Full;
            }
            else if (layoutType == Constants.LayoutType.Portrait_Full_TopMarquee)
            {
                //resPanel = mediaPanel_BR_Portrait_Full_TopMarquee;
            }
            else if (layoutType == Constants.LayoutType.Portrait_CenterMarquee_T1_B3)
            {
                //resPanel = mediaPanel_BR_Portrait_CenterMarquee_T1_B3;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_B1)
            {
                //resPanel = mediaPanel_BR_Portrait_T1_C1_B1;
            }
            else if (layoutType == Constants.LayoutType.Portrait_T1_C1_CL1_CR1_B1)
            {
                //resPanel = mediaPanel_BR_Portrait_T1_C1_CL1_CR1_B1;
            }

            return resPanel;
        }

        private String getString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
