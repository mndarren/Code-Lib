/*@author Zhao Xie <11/02/2013>
**@file Sort.h*/
//6 algorithms
#ifndef QUICK_SORT_H
#define QUICK_SORT_H
#include "LinkedQueue.h"
//quickSort
template <class ItemType>
void partition(ItemType a[], int low,int high,
               ItemType pivot,int &i,int &j);
template <class ItemType>
void quickSort(ItemType a[],int low,int high);
//insertionSort
template <class ItemType>
void insertionSort(ItemType a[],int low,int high);
//mergeSort
template <class ItemType>
void mergeSort(ItemType a[],int low,int high);
template <class ItemType>
void merge(ItemType a[],int low,int mid,int high);
//bubbleSort
template <class ItemType>
void bubbleSort(ItemType a[],int low,int high);
//selectionSort
template <class ItemType>
void selectionSort(ItemType a[],int low,int high);
template <class ItemType>
int findIndexofLargest(ItemType a[],int low,int high);
//radixSort
template <class ItemType>
void radixSort(ItemType a[],int low,int high);
template <class ItemType>
void radixS(ItemType a[],int low,int high,int radix,int digits);
//heapSort
template <class ItemType>
void heapSort(ItemType a[],int n);
template <class ItemType>
void walk_down(ItemType a[],int parent,int last);
//littleTools
template <class ItemType>
void exchange(ItemType& a,ItemType& b);
#include "Sort.cpp"
#endif