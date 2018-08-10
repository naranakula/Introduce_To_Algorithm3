using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Introduce_To_Algorithm3.Models
{

    /// <summary>
    /// ViewModel的基类
    /// </summary>
    public class MvvmBaseViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public MvvmBaseViewModel() { }

        /// <summary>
        /// 看情况设置volatile
        /// </summary>
        private volatile string _strExample;


        public string StrExample
        {
            get { return _strExample; }
            set
            {
                if (_strExample != value)
                {
                    _strExample = value;

                    //3、调用OnPropertyChanged的接口
                    // // OnPropertyChanged需要在UI线程上
                    OnPropertyChanged(nameof(StrExample));
                }
            }
        }


        #region 基类viewmodel

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var propertyChanged = PropertyChanged;
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }

}
