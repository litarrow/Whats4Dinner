﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using Whats4Dinner.Models.DataStructure;
using Xamarin.Forms;
using static Whats4Dinner.Models.DataStructure.Dish;

namespace Whats4Dinner.Models
{
	/// <summary>
	/// File Path interface to be implemented in each platform for dependency service
	/// </summary>
	public interface IFilePathService
	{
		string GetFilePath(string fileName);
	}

	/// <summary>
	/// Class to handle file I/O of the user data
	/// </summary>
	public class FileIO
	{
		/// <summary>
		/// Absolute file path used for file I/O
		/// </summary>
		private string FilePath { get; set; }

		/// <summary>
		/// JSON serializer options to enable usage of Dictionary data structure
		/// </summary>
		private JsonSerializerOptions serializeOptions = new JsonSerializerOptions();

		/// <summary>
		/// Create a sample json file for testing
		/// </summary>
		//public void CreateSampleUserData()
		//{
		//	// create the days object to save to JSON
		//	ObservableCollection<Day> sampleDays = new ObservableCollection<Day>
		//	{
		//		new Day(DateTime.Today.AddDays(1)),
		//		new Day(DateTime.Today.AddDays(3)),
		//		new Day(DateTime.Today.AddDays(5)),
		//		new Day(DateTime.Today.AddDays(6))
		//	};

		//	// fill the sample days
		//	sampleDays[0].Meals[(int)Meal.MealType.Breakfast].AddDish("Blueberry Pancakes", new List<string> { DishCategory.Grain});
		//	sampleDays[1].Meals[(int)Meal.MealType.Lunch].AddDish("Ribeye Steak", new List<string> { DishCategory.Protein });
		//	sampleDays[2].Meals[(int)Meal.MealType.Dinner].AddDish("Green Salad", new List<string> { DishCategory.Veggie });
		//	sampleDays[3].Meals[(int)Meal.MealType.Breakfast].AddDish("Blueberry Pancakes", new List<string> { DishCategory.Grain });
		//	sampleDays[3].Meals[(int)Meal.MealType.Lunch].AddDish("Ribeye Steak", new List<string> { DishCategory.Protein });
		//	sampleDays[3].Meals[(int)Meal.MealType.Dinner].AddDish("Green Salad", new List<string> { DishCategory.Veggie });

		//	WriteUserDataToJSON(sampleDays);
		//}

		//public void CreateSampleDishes()
		//{
		//	// list of sample dishes
		//	ObservableCollection<Dish> sampleDishes = new ObservableCollection<Dish>
		//	{
		//		new Dish("dish 1", new List<string> { DishCategory.Protein, DishCategory.Veggie }),
		//		new Dish("dish 2"),
		//		new Dish("dish 3"),
		//		new Dish("dish 4"),
		//		new Dish("dish 5"),
		//		new Dish("dish 6"),
		//		new Dish("dish 7"),
		//		new Dish("dish 8"),
		//		new Dish("dish 9")
		//	};

		//	WriteDishesToJSON(sampleDishes);
		//}

		/// <summary>
		/// Sort, convert DishGroup to DishGroupForJSON, and then write user's data to JSON file
		/// </summary>
		/// <param name="days"></param>
		public void WriteUserDataToJSON(ObservableCollection<Day> DisplayDays)
		{
			// convert ObservalbeCollection to List so it can sort
			List<Day> days = new List<Day>();
			foreach (Day day in DisplayDays)
			{
				days.Add(day);
			}

			// sort the list by day
			days = days.OrderBy(day => day.ThisDate).ToList();

			// save to file
			string jsonString = JsonSerializer.Serialize(days, serializeOptions);
			File.WriteAllText(FilePath, jsonString);
		}

		public void WriteDishesToJSON(ObservableCollection<Dish> Dishes)
		{
			// convert ObservalbeCollection to List so it can sort
			List<Dish> dishes = new List<Dish>();
			foreach (Dish day in Dishes)
			{
				dishes.Add(day);
			}

			// sort the list by name
			dishes = dishes.OrderBy(dish => dish.Name).ToList();

			// save to file
			string jsonString = JsonSerializer.Serialize(dishes, serializeOptions);
			File.WriteAllText(FilePath, jsonString);
		}

		public void WriteDishCategoriesToJSON(List<string> DishCategories)
		{
			// save to file
			string jsonString = JsonSerializer.Serialize(DishCategories, serializeOptions);
			File.WriteAllText(FilePath, jsonString);
		}

		/// <summary>
		/// Read user's data from JSON file, covnert DishGroupForJSON to DishGroup, and return it
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns>List<Day> object read from user's data file</Day></returns>
		public ObservableCollection<Day> ReadUserDataFromJSON()
		{
			ObservableCollection<Day> result;

			if (File.Exists(FilePath))
			{
				string jsonString = File.ReadAllText(FilePath);
				result = JsonSerializer.Deserialize<ObservableCollection<Day>>(jsonString, serializeOptions);
			}
			// return empty list if file does not exist
			else
			{
				result = new ObservableCollection<Day>();
			}

			return result;
		}

		public ObservableCollection<Dish> ReadDishesFromJSON()
		{
			ObservableCollection<Dish> result;

			if (File.Exists(FilePath))
			{
				string jsonString = File.ReadAllText(FilePath);
				result = JsonSerializer.Deserialize<ObservableCollection<Dish>>(jsonString, serializeOptions);
			}
			// return empty list if file does not exist
			else
			{
				result = new ObservableCollection<Dish>();
			}

			return result;
		}

		public List<string> ReadDishCategoriesFromJSON()
		{
			List<string> result;

			if (File.Exists(FilePath))
			{
				string jsonString = File.ReadAllText(FilePath);
				result = JsonSerializer.Deserialize<List<string>>(jsonString, serializeOptions);
			}
			// return a pre-made list if file does not exist
			else
			{
				result = new List<string>
				{
					"Grain",
					"Veggi",
					"Prote",
					"Condi",
					"Drink"
				};
			}

			return result;
		}

		public FileIO() { }

		/// <summary>
		/// Constructor for FilIO class; calculate and initialize FilePath from provided file name
		/// </summary>
		/// <param name="fileName"></param>
		public FileIO(string fileName)
		{
			// build file path
			IFilePathService service = DependencyService.Get<IFilePathService>();
			FilePath = service.GetFilePath(fileName);

			// add serializer options to be able to use Dictionary data structure in JSON
			serializeOptions.Converters.Add(new DictionaryTKeyEnumTValueConverter());
		}
	}
}
