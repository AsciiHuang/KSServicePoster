using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSServicePoster
{
    public class Constants
    {
        public enum LayoutType
        {
            Landscape_Full, 
            Landscape_L1_R21, 
            Landscape_L1_R111, 
            Landscape_Full_BottomMarquee, 
            Landscape_BottomMarquee_L1_R11, 
            Landscape_BottomMarquee_L1_R21, 
            Landscape_L1_C1_R1, 
            Landscape_BottomMarquee_L1_C1_R1, 
            Portrait_Full, 
            Portrait_Full_TopMarquee, 
            Portrait_CenterMarquee_T1_B3,
            Portrait_T1_C1_B1, 
            Portrait_T1_C1_CL1_CR1_B1, 
        }

        public enum MediaType
        {
            None, Photo, Video, 
        }

        public enum MediaDuration
        {
            None, 
            Second5, 
            Second10, 
            Second15, 
            Second20, 
            Second25,
            Second30,
            Second35,
            Second40,
            Second45,
            Second50,
            Second55,
            Second60,
            Unlimited, 
        }

        public enum MediaLayoutPosition
        {
            None, 
            TL, TC, TR, 
            CL, CC, CR, 
            BL, BC, BR, 
        }
    }
}
