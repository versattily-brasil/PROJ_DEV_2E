using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Versattily.WebDriver
{
	public class SimpleManage : IOptions
	{
		VersattilyDriver _my;
		public SimpleManage(VersattilyDriver driver)
		{
			_my = driver;
		}
		#region IOptions Members

		public ICookieJar Cookies
		{
			get { throw new NotImplementedException(); }
		}

		public ITimeouts Timeouts()
		{
			throw new NotImplementedException();
		}

		public IWindow Window
		{
			get { throw new NotImplementedException(); }
		}

        public ILogs Logs
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
