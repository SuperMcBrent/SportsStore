using SportsStore.BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStore.BAL {
    public class City : IStore {
        #region Fields
        private Contracts.City _data;
        private bool _isDirty;
        #endregion

        #region Properties
        public Contracts.City Data { get { return _data; } }
        public int Id { get { return _data.Id; } set { if (_data.Id != value) { _data.Id = value; _isDirty = true; } } }
        public string PostalCode { get { return _data.PostalCode; } set { if (_data.PostalCode != value) { _data.PostalCode = value; _isDirty = true; } } }
        public string Name { get { return _data.Name; } set { if (_data.Name != value) { _data.Name = value; _isDirty = true; } } }
        #endregion

        #region Ctor
        public City() {
            _isDirty = true;
            _data = new Contracts.City();
        }

        public City(Contracts.City city) {
            _data = city;
            var cityDAL = new DAL.City();
            if (cityDAL.Exists(city))
                Load();
            else
                _isDirty = true;
        }
        #endregion

        #region Methods
        #region IStore
        public void Load() {
            if (_data.Id == 0) return;
            DAL.City dal = new DAL.City();
            dal.Select(_data);
        }

        public void Save() {
            if (_isDirty) {
                DAL.City city = new DAL.City();
                if (!city.Exists(_data)) {
                    city.Insert(_data);
                } else {
                    city.Update(_data);
                }
            }
        }
        #endregion
        #endregion
    }
}
