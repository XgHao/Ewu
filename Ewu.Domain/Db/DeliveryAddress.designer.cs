﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ewu.Domain.Db
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="LinJiaoFengJu")]
	public partial class DeliveryAddressDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 可扩展性方法定义
    partial void OnCreated();
    #endregion
		
		public DeliveryAddressDataContext() : 
				base(global::Ewu.Domain.Properties.Settings.Default.LinJiaoFengJuConnectionString2, mappingSource)
		{
			OnCreated();
		}
		
		public DeliveryAddressDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DeliveryAddressDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DeliveryAddressDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DeliveryAddressDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<DeliveryAddress> DeliveryAddress
		{
			get
			{
				return this.GetTable<DeliveryAddress>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.DeliveryAddress")]
	public partial class DeliveryAddress
	{
		
		private string _DeliveryAddressUID;
		
		private string _UserUID;
		
		private string _LocationProvince;
		
		private string _LocationCity;
		
		private string _LocationDistrict;
		
		private string _MoreLocation;
		
		private string _PhoneNum;
		
		private string _RealName;
		
		public DeliveryAddress()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeliveryAddressUID", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string DeliveryAddressUID
		{
			get
			{
				return this._DeliveryAddressUID;
			}
			set
			{
				if ((this._DeliveryAddressUID != value))
				{
					this._DeliveryAddressUID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserUID", DbType="VarChar(50)")]
		public string UserUID
		{
			get
			{
				return this._UserUID;
			}
			set
			{
				if ((this._UserUID != value))
				{
					this._UserUID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LocationProvince", DbType="NVarChar(50)")]
		public string LocationProvince
		{
			get
			{
				return this._LocationProvince;
			}
			set
			{
				if ((this._LocationProvince != value))
				{
					this._LocationProvince = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LocationCity", DbType="NVarChar(50)")]
		public string LocationCity
		{
			get
			{
				return this._LocationCity;
			}
			set
			{
				if ((this._LocationCity != value))
				{
					this._LocationCity = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LocationDistrict", DbType="NVarChar(50)")]
		public string LocationDistrict
		{
			get
			{
				return this._LocationDistrict;
			}
			set
			{
				if ((this._LocationDistrict != value))
				{
					this._LocationDistrict = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MoreLocation", DbType="NVarChar(MAX)")]
		public string MoreLocation
		{
			get
			{
				return this._MoreLocation;
			}
			set
			{
				if ((this._MoreLocation != value))
				{
					this._MoreLocation = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PhoneNum", DbType="NVarChar(50)")]
		public string PhoneNum
		{
			get
			{
				return this._PhoneNum;
			}
			set
			{
				if ((this._PhoneNum != value))
				{
					this._PhoneNum = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RealName", DbType="NVarChar(50)")]
		public string RealName
		{
			get
			{
				return this._RealName;
			}
			set
			{
				if ((this._RealName != value))
				{
					this._RealName = value;
				}
			}
		}
	}
}
#pragma warning restore 1591