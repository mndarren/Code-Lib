using System;
using System.Collections.Generic;

public class Tester {
  public static void Main (string[] args) {
    UnitTest.TestTwoSum(); // Test {1, 5, 19, 43, 58, 99} and 24
    UnitTest.TestTwoSumB(); // Test {2, 7, 11, 15, 21} and 28
  }
}

// Given an array of integers, return indices of the two numbers 
// such that they add up to a specific target.
//
// You may assume that each input would have exactly one solution, 
// and you may not use the same element twice.
//
// Example:
//
// Given nums = [2, 7, 11, 15], target = 9,
//
// Because nums[0] + nums[1] = 2 + 7 = 9,
// return [0, 1].
//
public class Example {
    public int[] TwoSum(int[] nums, int target) {
        
        Dictionary<int, int> map = new Dictionary<int, int>();
        for(int i = 0; i < nums.Length; i++) {
          int key1 = nums[i];
          int key2 = target - nums[i];
          if(map.ContainsKey(key2)) {
            return new int[] {map[key2], i};
          }
          map.Add(key1, i);
        }
        throw new Exception("Not Found the target.");
    }
} 