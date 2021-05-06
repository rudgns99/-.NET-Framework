using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace GWF
{
    public class ControlHelper
    {
        /// <summary>
        /// 컨트롤을 로드할 때, 파라미터를 넘길 수 있음. 단, 페이지에서 생성자를 갖추어야 함
        /// </summary>
        /// <param name="Page">사용할려는 Page객체</param>
        /// <param name="UserControlPath">유저컨트롤 경로</param>
        /// <param name="Parameters">생성자와 같은 수, 같은 타입의 파라미터(순서대로 입력해야함)</param>
        /// <returns></returns>
        public static UserControl LoadControl(System.Web.UI.Page Page, string UserControlPath, params object[] Parameters)
        {
            List<System.Type> constParamTypes = new List<System.Type>();
            foreach (object constParam in Parameters)
            {
                constParamTypes.Add(constParam.GetType());
            }

            UserControl ctl = Page.LoadControl(UserControlPath) as UserControl;

            // Find the relevant constructor
            ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());

            //And then call the relevant constructor
            if (constructor == null)
            {
                throw new MemberAccessException("생성자를 찾을 수 없습니다. : " + ctl.GetType().BaseType.ToString());
            }
            else
            {
                constructor.Invoke(ctl, Parameters);
            }

            // Finally return the fully initialized UC
            return ctl;
        }
    }
}
