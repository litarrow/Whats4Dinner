﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static Whats4Dinner.Models.DataStructure.Dish;

namespace Whats4Dinner.Models.DataStructure
{
	/// <summary>
	/// A meal with MealType, a list of Dishes, a string of dishes to display on View
	/// </summary>
	public class Meal : BaseModel
	{
		/// <summary>
		/// List of types a dish can have
		/// </summary>
		public enum MealType
		{
			Breakfast,
			Lunch,
			Dinner,
			Other
		}

		/// <summary>
		/// Type of the Meal, such as breakfast, lunch, dinner, etc.
		/// </summary>
		public MealType ThisMealType
		{
			get => thisMealType;
			set => SetProperty(ref thisMealType, value);
		}

		Dictionary<string, object> UserData;

		/// <summary>
		/// List of dishes in the meal, separated by categories, such as grains, veggies, proteins, etc.
		/// </summary>
		public ObservableCollection<Dish> Dishes
		{
			get => dishes;
			set => SetProperty(ref dishes, value);
		}

		/// <summary>
		/// Whether this meal has any dishes
		/// </summary>
		public bool HasDishes
		{
			get => hasDishes;
			set =>SetProperty(ref hasDishes, value);
		}

		/// <summary>
		/// Whether this meal has no dishes
		/// </summary>
		public bool NoDishes
		{
			get => noDishes;
			set => SetProperty(ref noDishes, value);
		}

		/// <summary>
		/// List of dishes for display in View
		/// </summary>
		public string DisplayPreviewDishes
		{
			get
			{
				string mealString = "";

				// for each dish
				foreach (Dish dish in Dishes)
				{
					mealString += dish.Name;

					// also add the dish categories
					if (dish.ThisDishCategories.Count > 0)
					{
						mealString += " (";

						foreach (string dishCategory in dish.ThisDishCategories)
						{
							mealString += dishCategory;
							if (dishCategory != dish.ThisDishCategories.Last())
							{
								mealString += ", ";
							}
						}
						mealString += ")";
					}

					if (dish != Dishes.Last())
					{
						mealString += ", ";
					}
				}

				SetProperty(ref displayPreviewDishes, mealString);
				return displayPreviewDishes;
			}
			set
			{
				SetProperty(ref displayPreviewDishes, value);
			}
		}

		// fields for the properties above
		private MealType thisMealType;
		private ObservableCollection<Dish> dishes = new ObservableCollection<Dish>();
		private bool hasDishes = false;
		private bool noDishes = true;
		private string displayPreviewDishes;

		/// <summary>
		/// Add a Dish to the list of Dishes in the provided dish category
		/// </summary>
		/// <param name="name"></param>
		/// <param name="cat"></param>
		public void AddDish(string name, List<string> cat, Dictionary<string, object> UserData)
		{
			Dishes.Add(new Dish(name, cat, UserData));
			NoDishes = Dishes.Count == 0;
			HasDishes = !NoDishes;
		}

		/// <summary>
		/// Edit an existing Dish
		/// </summary>
		/// <param name="dish"></param>
		/// <param name="name"></param>
		/// <param name="cat"></param>
		public void EditDish(Dish dish, string name, List<string> cat)
		{
			dish.Name = name;
			dish.ThisDishCategories = cat;
			OnPropertyChanged("DisplayPreviewDishes");
		}

		/// <summary>
		/// Delete a Dish from the list of Dishes
		/// </summary>
		/// <param name="selected"></param>
		public void RemoveDish(Dish selected)
		{
			Dishes.Remove(selected);
			NoDishes = Dishes.Count == 0;
			HasDishes = !NoDishes;
		}

		/// <summary>
		/// Parameterless constructor for JSON deserialization
		/// </summary>
		public Meal() { }

		/// <summary>
		/// Constructor for Meal class
		/// </summary>
		/// <param name="mealType">Type of the Meal, such as breakfast, lunch, dinner, etc.</param>
		public Meal(MealType mealType, Dictionary<string, object> UserData)
		{
			// initialize properties
			this.UserData = UserData;
			ThisMealType = mealType;
		}
	}
}
