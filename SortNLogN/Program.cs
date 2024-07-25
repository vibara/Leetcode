using System.Runtime.InteropServices;

int[] array = [5, 1, 1, 2, 0, 0];
var solution = new Solution();
var sorted = solution.SortArray(array);

for (int i = 0; i < sorted.Length; i++) {
    if (i > 0) {
        Console.Write(", ");
    }
    Console.Write(sorted[i]);
}
Console.WriteLine();

public class Solution {
    private int[] MergeArrays(int[] left, int[] right) {
        var leftSize = left.Length;
        var rightSize = right.Length;
        var newSize = leftSize + rightSize;
        var result = new int[newSize];
        int iLeft = 0, iRight = 0;
        for (int j = 0; j < newSize; j++) {
            if (iLeft < leftSize && iRight < rightSize) {
                if (left[iLeft] < right[iRight]) {
                    result[j] = left[iLeft];
                    iLeft++;
                }
                else {
                    result[j] = right[iRight];
                    iRight++;
                }
            }
            else {
                if (iLeft < leftSize) {
                    result[j] = left[iLeft];
                    iLeft++;
                }
                else if (iRight < rightSize) {  // we can ignore else
                    result[j] = right[iRight];
                    iRight++;
                }
            }
        }
        return result;
    }

    private int[] Sort(int[] array) {
        if (array.Length < 2) {
            return array;
        }
        int len1 = array.Length / 2;
        int len2 = array.Length - len1;
        var arr1 = new int[len1];
        var arr2 = new int[len2];
        Array.Copy(array, 0, arr1, 0, len1);
        Array.Copy(array, len1, arr2, 0, len2);
        return MergeArrays(Sort(arr1), Sort(arr2));
    }

    public int[] SortArray(int[] nums) {
        // merge sort  
        return Sort(nums);
    }
}