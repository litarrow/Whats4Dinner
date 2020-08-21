﻿using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Whats4Dinner.Models;
using Whats4Dinner.Models.DataStructure;

namespace Whats4Dinner.ViewModels
{
	/// <summary>
	/// The base class for all ViewModels which use properties for the Views such as Title, IsBusy, FilIO, etc.
	/// This also includes the list of Days used throughout the app.
	/// </summary>
	public class BaseViewModel : BaseModel
	{
		/// <summary>
		/// The file name for storing the user data
		/// </summary>
		protected static readonly string userFileName = "UserData.json";

		/// <summary>
		/// The file name for storing the dish database
		/// </summary>
		protected static readonly string dishFileName = "DishDB.json";

		/// <summary>
		/// DelegateCommand (from Prism) to use in View when refreshing
		/// </summary>
		public DelegateCommand LoadItemsCommand { get; set; }

		/// <summary>
		/// FileIO object to handle user data I/O
		/// </summary>
		protected FileIO UserDataIO { get; set; }

		/// <summary>
		/// FileIO object to handle dish database I/O
		/// </summary>
		protected FileIO DishDBIO { get; set; }
			
		/// <summary>
							/// The title of the page
							/// </summary>
		public string Title
		{
			get => title;
			set { SetProperty(ref title, value); }
		}

		/// <summary>
		/// Whether the page is actively refreshing in the RefreshView
		/// </summary>
		public bool IsBusy
		{
			get => isBusy;
			set { SetProperty(ref isBusy, value); }
		}

		/// <summary>
		/// List of all the Days and their Meals and Dishes
		/// </summary>
		public ObservableCollection<Day> DisplayDays
		{
			get => displayDays;
			set
			{
				SetProperty(ref displayDays, value);
			}
		}

		/// <summary>
		/// List of Dishes already created before
		/// </summary>
		public ObservableCollection<Dish> DishDB
		{
			get => dishDB;
			set
			{
				SetProperty(ref dishDB, value);
			}
		}

		// fields for the properties above
		private string title = string.Empty;
		private bool isBusy = false;
		private ObservableCollection<Day> displayDays;
		private ObservableCollection<Dish> dishDB;

		/// <summary>
		/// Fill the DisplayDays from user data read from file
		/// </summary>
		/// <param name="dataFromFile"></param>
		protected void FillDisplayDays(ObservableCollection<Day> dataFromFile)
		{
			DateTime today = DateTime.Today;
			int i = 0, j = 0;

			while (i < 7)
			{
				DateTime fileDate, currentDate = today.AddDays(i);

				// prevent j from going out of bounds
				if (j < dataFromFile.Count)
				{
					fileDate = dataFromFile[j].ThisDate;
				}
				else
				{
					fileDate = today.AddDays(7);
				}

				// if we run out of data from file, fill days with blanks
				if (j > dataFromFile.Count - 1)
				{
					DisplayDays.Add(new Day(currentDate));
					i++;
				}
				// skip until today
				else if (fileDate < currentDate)
				{
					j++;
				}
				// use the day if date matches
				else if (fileDate == currentDate)
				{
					DisplayDays.Add(dataFromFile[j]);
					j++;
					i++;
				}
				// fill the between days with empty day
				else if (fileDate <= currentDate.AddDays(6))
				{
					int emptyDays = (fileDate - currentDate).Days;
					for (int k = 0; k < emptyDays; k++)
					{
						DisplayDays.Add(new Day(currentDate));
						i++;
					}
				}
				// ignore all dates after the 7 days
				else
				{
					j = dataFromFile.Count - 1;
				}
			}
		}

		/// <summary>
		/// Replace the DisplayDays with new data from the file
		/// (to be made asyncronous when using database)
		/// </summary>
		protected void ExecuteLoadItemsCommand()
		{
			IsBusy = true;

			try
			{
				DisplayDays.Clear();

				// read user's data from JSON file
				ObservableCollection<Day> dataFromFile = UserDataIO.ReadUserDataFromJSON();

				// fill the week with days
				FillDisplayDays(dataFromFile);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}
