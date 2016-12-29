using EmitMapper;
using EmitMapper.MappingConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Common
{
    public class OOMapper
    {
        ObjectMapperManager _mapper;
        private volatile static OOMapper _instance = null;

        private static readonly object lockHelper = new object();

        public static OOMapper Instance()
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                    {
                        _instance = new OOMapper();
                    }
                }
            }
            return _instance;
        }
        public OOMapper()
        {
            _mapper = ObjectMapperManager.DefaultInstance;
        }
        public TTo Map<TForm, TTo>(TForm source, DefaultMapConfig conf = null) where TForm : class where TTo : class
        {
            TTo toObj = null;

            if (conf == null) return _mapper.GetMapper<TForm, TTo>().Map(source);

            toObj = _mapper.GetMapper<TForm, TTo>(conf).Map(source);

            return toObj;
        }

    }
}
