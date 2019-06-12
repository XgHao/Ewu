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
	public partial class AspNetUserDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 可扩展性方法定义
    partial void OnCreated();
    partial void InsertAspNetUsers(AspNetUsers instance);
    partial void UpdateAspNetUsers(AspNetUsers instance);
    partial void DeleteAspNetUsers(AspNetUsers instance);
    #endregion
		
		public AspNetUserDataContext() : 
				base(global::Ewu.Domain.Properties.Settings.Default.LinJiaoFengJuConnectionString1, mappingSource)
		{
			OnCreated();
		}
		
		public AspNetUserDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AspNetUserDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AspNetUserDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AspNetUserDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<AspNetUsers> AspNetUsers
		{
			get
			{
				return this.GetTable<AspNetUsers>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.AspNetUsers")]
	public partial class AspNetUsers : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _Id;
		
		private string _Email;
		
		private bool _EmailConfirmed;
		
		private string _PasswordHash;
		
		private string _SecurityStamp;
		
		private string _PhoneNumber;
		
		private bool _PhoneNumberConfirmed;
		
		private bool _TwoFactorEnabled;
		
		private System.Nullable<System.DateTime> _LockoutEndDateUtc;
		
		private bool _LockoutEnabled;
		
		private int _AccessFailedCount;
		
		private string _UserName;
		
		private string _HeadPortrait;
		
		private string _Gender;
		
		private string _Signature;
		
		private string _RealName;
		
		private System.DateTime _BirthDay;
		
		private int _Age;
		
		private string _IDCardNO;
		
		private string _NativePlace;
		
		private string _OICQ;
		
		private string _WeChat;
		
		private string _Job;
		
		private System.DateTime _RegisterTime;
		
		private string _IsShowInfo;
		
		private decimal _CreditWorthiness;
		
		private decimal _TempDeductionValue;
		
		private decimal _MultipleDeduct;
		
		private int _PenaltyTime;
		
		private int _Notice;
		
		private int _Favorite;
		
		private System.Data.Linq.Binary _IDCardImageData;
		
		private string _IDCardImageMimeType;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(string value);
    partial void OnIdChanged();
    partial void OnEmailChanging(string value);
    partial void OnEmailChanged();
    partial void OnEmailConfirmedChanging(bool value);
    partial void OnEmailConfirmedChanged();
    partial void OnPasswordHashChanging(string value);
    partial void OnPasswordHashChanged();
    partial void OnSecurityStampChanging(string value);
    partial void OnSecurityStampChanged();
    partial void OnPhoneNumberChanging(string value);
    partial void OnPhoneNumberChanged();
    partial void OnPhoneNumberConfirmedChanging(bool value);
    partial void OnPhoneNumberConfirmedChanged();
    partial void OnTwoFactorEnabledChanging(bool value);
    partial void OnTwoFactorEnabledChanged();
    partial void OnLockoutEndDateUtcChanging(System.Nullable<System.DateTime> value);
    partial void OnLockoutEndDateUtcChanged();
    partial void OnLockoutEnabledChanging(bool value);
    partial void OnLockoutEnabledChanged();
    partial void OnAccessFailedCountChanging(int value);
    partial void OnAccessFailedCountChanged();
    partial void OnUserNameChanging(string value);
    partial void OnUserNameChanged();
    partial void OnHeadPortraitChanging(string value);
    partial void OnHeadPortraitChanged();
    partial void OnGenderChanging(string value);
    partial void OnGenderChanged();
    partial void OnSignatureChanging(string value);
    partial void OnSignatureChanged();
    partial void OnRealNameChanging(string value);
    partial void OnRealNameChanged();
    partial void OnBirthDayChanging(System.DateTime value);
    partial void OnBirthDayChanged();
    partial void OnAgeChanging(int value);
    partial void OnAgeChanged();
    partial void OnIDCardNOChanging(string value);
    partial void OnIDCardNOChanged();
    partial void OnNativePlaceChanging(string value);
    partial void OnNativePlaceChanged();
    partial void OnOICQChanging(string value);
    partial void OnOICQChanged();
    partial void OnWeChatChanging(string value);
    partial void OnWeChatChanged();
    partial void OnJobChanging(string value);
    partial void OnJobChanged();
    partial void OnRegisterTimeChanging(System.DateTime value);
    partial void OnRegisterTimeChanged();
    partial void OnIsShowInfoChanging(string value);
    partial void OnIsShowInfoChanged();
    partial void OnCreditWorthinessChanging(decimal value);
    partial void OnCreditWorthinessChanged();
    partial void OnTempDeductionValueChanging(decimal value);
    partial void OnTempDeductionValueChanged();
    partial void OnMultipleDeductChanging(decimal value);
    partial void OnMultipleDeductChanged();
    partial void OnPenaltyTimeChanging(int value);
    partial void OnPenaltyTimeChanged();
    partial void OnNoticeChanging(int value);
    partial void OnNoticeChanged();
    partial void OnFavoriteChanging(int value);
    partial void OnFavoriteChanged();
    partial void OnIDCardImageDataChanging(System.Data.Linq.Binary value);
    partial void OnIDCardImageDataChanged();
    partial void OnIDCardImageMimeTypeChanging(string value);
    partial void OnIDCardImageMimeTypeChanged();
    #endregion
		
		public AspNetUsers()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="NVarChar(128) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Email", DbType="NVarChar(256)")]
		public string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				if ((this._Email != value))
				{
					this.OnEmailChanging(value);
					this.SendPropertyChanging();
					this._Email = value;
					this.SendPropertyChanged("Email");
					this.OnEmailChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EmailConfirmed", DbType="Bit NOT NULL")]
		public bool EmailConfirmed
		{
			get
			{
				return this._EmailConfirmed;
			}
			set
			{
				if ((this._EmailConfirmed != value))
				{
					this.OnEmailConfirmedChanging(value);
					this.SendPropertyChanging();
					this._EmailConfirmed = value;
					this.SendPropertyChanged("EmailConfirmed");
					this.OnEmailConfirmedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PasswordHash", DbType="NVarChar(MAX)")]
		public string PasswordHash
		{
			get
			{
				return this._PasswordHash;
			}
			set
			{
				if ((this._PasswordHash != value))
				{
					this.OnPasswordHashChanging(value);
					this.SendPropertyChanging();
					this._PasswordHash = value;
					this.SendPropertyChanged("PasswordHash");
					this.OnPasswordHashChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SecurityStamp", DbType="NVarChar(MAX)")]
		public string SecurityStamp
		{
			get
			{
				return this._SecurityStamp;
			}
			set
			{
				if ((this._SecurityStamp != value))
				{
					this.OnSecurityStampChanging(value);
					this.SendPropertyChanging();
					this._SecurityStamp = value;
					this.SendPropertyChanged("SecurityStamp");
					this.OnSecurityStampChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PhoneNumber", DbType="NVarChar(MAX)")]
		public string PhoneNumber
		{
			get
			{
				return this._PhoneNumber;
			}
			set
			{
				if ((this._PhoneNumber != value))
				{
					this.OnPhoneNumberChanging(value);
					this.SendPropertyChanging();
					this._PhoneNumber = value;
					this.SendPropertyChanged("PhoneNumber");
					this.OnPhoneNumberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PhoneNumberConfirmed", DbType="Bit NOT NULL")]
		public bool PhoneNumberConfirmed
		{
			get
			{
				return this._PhoneNumberConfirmed;
			}
			set
			{
				if ((this._PhoneNumberConfirmed != value))
				{
					this.OnPhoneNumberConfirmedChanging(value);
					this.SendPropertyChanging();
					this._PhoneNumberConfirmed = value;
					this.SendPropertyChanged("PhoneNumberConfirmed");
					this.OnPhoneNumberConfirmedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TwoFactorEnabled", DbType="Bit NOT NULL")]
		public bool TwoFactorEnabled
		{
			get
			{
				return this._TwoFactorEnabled;
			}
			set
			{
				if ((this._TwoFactorEnabled != value))
				{
					this.OnTwoFactorEnabledChanging(value);
					this.SendPropertyChanging();
					this._TwoFactorEnabled = value;
					this.SendPropertyChanged("TwoFactorEnabled");
					this.OnTwoFactorEnabledChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LockoutEndDateUtc", DbType="DateTime")]
		public System.Nullable<System.DateTime> LockoutEndDateUtc
		{
			get
			{
				return this._LockoutEndDateUtc;
			}
			set
			{
				if ((this._LockoutEndDateUtc != value))
				{
					this.OnLockoutEndDateUtcChanging(value);
					this.SendPropertyChanging();
					this._LockoutEndDateUtc = value;
					this.SendPropertyChanged("LockoutEndDateUtc");
					this.OnLockoutEndDateUtcChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LockoutEnabled", DbType="Bit NOT NULL")]
		public bool LockoutEnabled
		{
			get
			{
				return this._LockoutEnabled;
			}
			set
			{
				if ((this._LockoutEnabled != value))
				{
					this.OnLockoutEnabledChanging(value);
					this.SendPropertyChanging();
					this._LockoutEnabled = value;
					this.SendPropertyChanged("LockoutEnabled");
					this.OnLockoutEnabledChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AccessFailedCount", DbType="Int NOT NULL")]
		public int AccessFailedCount
		{
			get
			{
				return this._AccessFailedCount;
			}
			set
			{
				if ((this._AccessFailedCount != value))
				{
					this.OnAccessFailedCountChanging(value);
					this.SendPropertyChanging();
					this._AccessFailedCount = value;
					this.SendPropertyChanged("AccessFailedCount");
					this.OnAccessFailedCountChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="NVarChar(256) NOT NULL", CanBeNull=false)]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this.OnUserNameChanging(value);
					this.SendPropertyChanging();
					this._UserName = value;
					this.SendPropertyChanged("UserName");
					this.OnUserNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HeadPortrait", DbType="NVarChar(MAX)")]
		public string HeadPortrait
		{
			get
			{
				return this._HeadPortrait;
			}
			set
			{
				if ((this._HeadPortrait != value))
				{
					this.OnHeadPortraitChanging(value);
					this.SendPropertyChanging();
					this._HeadPortrait = value;
					this.SendPropertyChanged("HeadPortrait");
					this.OnHeadPortraitChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Gender", DbType="NVarChar(50)")]
		public string Gender
		{
			get
			{
				return this._Gender;
			}
			set
			{
				if ((this._Gender != value))
				{
					this.OnGenderChanging(value);
					this.SendPropertyChanging();
					this._Gender = value;
					this.SendPropertyChanged("Gender");
					this.OnGenderChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Signature", DbType="NVarChar(MAX)")]
		public string Signature
		{
			get
			{
				return this._Signature;
			}
			set
			{
				if ((this._Signature != value))
				{
					this.OnSignatureChanging(value);
					this.SendPropertyChanging();
					this._Signature = value;
					this.SendPropertyChanged("Signature");
					this.OnSignatureChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RealName", DbType="NVarChar(MAX)")]
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
					this.OnRealNameChanging(value);
					this.SendPropertyChanging();
					this._RealName = value;
					this.SendPropertyChanged("RealName");
					this.OnRealNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BirthDay", DbType="DateTime NOT NULL")]
		public System.DateTime BirthDay
		{
			get
			{
				return this._BirthDay;
			}
			set
			{
				if ((this._BirthDay != value))
				{
					this.OnBirthDayChanging(value);
					this.SendPropertyChanging();
					this._BirthDay = value;
					this.SendPropertyChanged("BirthDay");
					this.OnBirthDayChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Age", DbType="Int NOT NULL")]
		public int Age
		{
			get
			{
				return this._Age;
			}
			set
			{
				if ((this._Age != value))
				{
					this.OnAgeChanging(value);
					this.SendPropertyChanging();
					this._Age = value;
					this.SendPropertyChanged("Age");
					this.OnAgeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IDCardNO", DbType="NVarChar(MAX)")]
		public string IDCardNO
		{
			get
			{
				return this._IDCardNO;
			}
			set
			{
				if ((this._IDCardNO != value))
				{
					this.OnIDCardNOChanging(value);
					this.SendPropertyChanging();
					this._IDCardNO = value;
					this.SendPropertyChanged("IDCardNO");
					this.OnIDCardNOChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NativePlace", DbType="NVarChar(MAX)")]
		public string NativePlace
		{
			get
			{
				return this._NativePlace;
			}
			set
			{
				if ((this._NativePlace != value))
				{
					this.OnNativePlaceChanging(value);
					this.SendPropertyChanging();
					this._NativePlace = value;
					this.SendPropertyChanged("NativePlace");
					this.OnNativePlaceChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OICQ", DbType="NVarChar(MAX)")]
		public string OICQ
		{
			get
			{
				return this._OICQ;
			}
			set
			{
				if ((this._OICQ != value))
				{
					this.OnOICQChanging(value);
					this.SendPropertyChanging();
					this._OICQ = value;
					this.SendPropertyChanged("OICQ");
					this.OnOICQChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_WeChat", DbType="NVarChar(MAX)")]
		public string WeChat
		{
			get
			{
				return this._WeChat;
			}
			set
			{
				if ((this._WeChat != value))
				{
					this.OnWeChatChanging(value);
					this.SendPropertyChanging();
					this._WeChat = value;
					this.SendPropertyChanged("WeChat");
					this.OnWeChatChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Job", DbType="NVarChar(MAX)")]
		public string Job
		{
			get
			{
				return this._Job;
			}
			set
			{
				if ((this._Job != value))
				{
					this.OnJobChanging(value);
					this.SendPropertyChanging();
					this._Job = value;
					this.SendPropertyChanged("Job");
					this.OnJobChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RegisterTime", DbType="DateTime NOT NULL")]
		public System.DateTime RegisterTime
		{
			get
			{
				return this._RegisterTime;
			}
			set
			{
				if ((this._RegisterTime != value))
				{
					this.OnRegisterTimeChanging(value);
					this.SendPropertyChanging();
					this._RegisterTime = value;
					this.SendPropertyChanged("RegisterTime");
					this.OnRegisterTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsShowInfo", DbType="NVarChar(MAX)")]
		public string IsShowInfo
		{
			get
			{
				return this._IsShowInfo;
			}
			set
			{
				if ((this._IsShowInfo != value))
				{
					this.OnIsShowInfoChanging(value);
					this.SendPropertyChanging();
					this._IsShowInfo = value;
					this.SendPropertyChanged("IsShowInfo");
					this.OnIsShowInfoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreditWorthiness", DbType="Decimal(18,2) NOT NULL")]
		public decimal CreditWorthiness
		{
			get
			{
				return this._CreditWorthiness;
			}
			set
			{
				if ((this._CreditWorthiness != value))
				{
					this.OnCreditWorthinessChanging(value);
					this.SendPropertyChanging();
					this._CreditWorthiness = value;
					this.SendPropertyChanged("CreditWorthiness");
					this.OnCreditWorthinessChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TempDeductionValue", DbType="Decimal(18,2) NOT NULL")]
		public decimal TempDeductionValue
		{
			get
			{
				return this._TempDeductionValue;
			}
			set
			{
				if ((this._TempDeductionValue != value))
				{
					this.OnTempDeductionValueChanging(value);
					this.SendPropertyChanging();
					this._TempDeductionValue = value;
					this.SendPropertyChanged("TempDeductionValue");
					this.OnTempDeductionValueChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MultipleDeduct", DbType="Decimal(18,2) NOT NULL")]
		public decimal MultipleDeduct
		{
			get
			{
				return this._MultipleDeduct;
			}
			set
			{
				if ((this._MultipleDeduct != value))
				{
					this.OnMultipleDeductChanging(value);
					this.SendPropertyChanging();
					this._MultipleDeduct = value;
					this.SendPropertyChanged("MultipleDeduct");
					this.OnMultipleDeductChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PenaltyTime", DbType="Int NOT NULL")]
		public int PenaltyTime
		{
			get
			{
				return this._PenaltyTime;
			}
			set
			{
				if ((this._PenaltyTime != value))
				{
					this.OnPenaltyTimeChanging(value);
					this.SendPropertyChanging();
					this._PenaltyTime = value;
					this.SendPropertyChanged("PenaltyTime");
					this.OnPenaltyTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Notice", DbType="Int NOT NULL")]
		public int Notice
		{
			get
			{
				return this._Notice;
			}
			set
			{
				if ((this._Notice != value))
				{
					this.OnNoticeChanging(value);
					this.SendPropertyChanging();
					this._Notice = value;
					this.SendPropertyChanged("Notice");
					this.OnNoticeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Favorite", DbType="Int NOT NULL")]
		public int Favorite
		{
			get
			{
				return this._Favorite;
			}
			set
			{
				if ((this._Favorite != value))
				{
					this.OnFavoriteChanging(value);
					this.SendPropertyChanging();
					this._Favorite = value;
					this.SendPropertyChanged("Favorite");
					this.OnFavoriteChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IDCardImageData", DbType="VarBinary(MAX)", UpdateCheck=UpdateCheck.Never)]
		public System.Data.Linq.Binary IDCardImageData
		{
			get
			{
				return this._IDCardImageData;
			}
			set
			{
				if ((this._IDCardImageData != value))
				{
					this.OnIDCardImageDataChanging(value);
					this.SendPropertyChanging();
					this._IDCardImageData = value;
					this.SendPropertyChanged("IDCardImageData");
					this.OnIDCardImageDataChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IDCardImageMimeType", DbType="NVarChar(MAX)")]
		public string IDCardImageMimeType
		{
			get
			{
				return this._IDCardImageMimeType;
			}
			set
			{
				if ((this._IDCardImageMimeType != value))
				{
					this.OnIDCardImageMimeTypeChanging(value);
					this.SendPropertyChanging();
					this._IDCardImageMimeType = value;
					this.SendPropertyChanged("IDCardImageMimeType");
					this.OnIDCardImageMimeTypeChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
