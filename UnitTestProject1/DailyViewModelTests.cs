﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Whats4Dinner.ViewModels;

namespace UnitTestProject1
{
	[TestClass]
	public class DailyViewModelTests
	{
		[TestMethod]
		public void ConstructorTest()
		{
			// Arrange

			// Act
			SevenDayViewModel testDailyViewModel = new SevenDayViewModel();

			// Assert
			Assert.AreEqual(7, testDailyViewModel.UserDays.Count);
		}

		[TestMethod]
		public void DaysTest()
		{
			// Arrange
			string today = DateTime.Today.Date.ToString();

			// Act
			SevenDayViewModel testDailyViewModel = new SevenDayViewModel();

			// Assert
			Assert.AreEqual(today, testDailyViewModel.UserDays[0].DisplayDayOfWeek);
		}
	}
}
