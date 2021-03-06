﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotKeys
{
	public class HotKey
	{
		// Fields
		private Modifiers modifiers;
		private Key key;

		// Constructors 
		public HotKey()
		{
			IsEnabled = true;
		}
		
		// Events
		public event EventHandler<HotKeyFiredEventArgs> Fired;

		// Properties
		public int? Id { get; internal set; }	
		public bool IsEnabled { get; set; }
		public bool IsRegistered { get; internal set; }
		public Modifiers Modifiers
		{
			get { return this.modifiers; }
			set
			{
				if (!IsRegistered)
				{
					this.modifiers = value;
				}
				else
				{
					throw new HotKeyModificationException("Cannot change modifiers while registered.");
				}
			}
		}

		public Key Key
		{
			get { return this.key; }
			set
			{
				if (!IsRegistered)
				{
					this.key = value;
				}
				else
				{
					throw new HotKeyModificationException("Cannot change key while registered.");
				}
			}
		}

		// Methods
		internal void OnFired()
		{
			if (IsEnabled)
			{
				Fired?.Invoke(this, new HotKeyFiredEventArgs((int)Id, Modifiers, Key));
			}
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode() ^ Modifiers.GetHashCode() ^ Key.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj.GetHashCode() == this.GetHashCode())
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
