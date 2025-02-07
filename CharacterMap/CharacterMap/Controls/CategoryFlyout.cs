﻿using CharacterMap.Helpers;
using CharacterMap.ViewModels;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CharacterMap.Controls
{
    public sealed class CategoryFlyout : Control
    {
        public event EventHandler<IList<UnicodeCategoryModel>> AcceptClicked;

        public object SourceCategories
        {
            get { return (object)GetValue(SourceCategoriesProperty); }
            set { SetValue(SourceCategoriesProperty, value); }
        }

        public static readonly DependencyProperty SourceCategoriesProperty =
            DependencyProperty.Register(nameof(SourceCategories), typeof(object), typeof(CategoryFlyout), new PropertyMetadata(null));

        public Flyout Flyout
        {
            get { return (Flyout)GetValue(FlyoutProperty); }
            set { SetValue(FlyoutProperty, value); }
        }

        public static readonly DependencyProperty FlyoutProperty =
            DependencyProperty.Register(nameof(Flyout), typeof(Flyout), typeof(CategoryFlyout), new PropertyMetadata(null));

        private Button _appBtnSelectAll = null;
        private Button _appBtnClear = null;
        private Button _appBtnReset = null;
        private Button _btnApply = null;
        private ListViewBase _categoryList = null;

        private object _listSrc = null;

        public CategoryFlyout()
        {
            this.DefaultStyleKey = typeof(CategoryFlyout);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _categoryList ??= this.GetTemplateChild("CategoryList") as ListViewBase;

            if (_appBtnSelectAll is null && this.GetTemplateChild("AppBtnSelectAll") is Button sa)
            {
                _appBtnSelectAll = sa;
                sa.Click -= FilterSelectAll_Click;
                sa.Click += FilterSelectAll_Click;
            }

            if (_appBtnClear is null && this.GetTemplateChild("AppBtnClear") is Button cb)
            {
                _appBtnClear = cb;
                cb.Click -= FilterClear_Click;
                cb.Click += FilterClear_Click;
            }

            if (_appBtnReset is null && this.GetTemplateChild("AppBtnReset") is Button br)
            {
                _appBtnReset = br;
                br.Click -= FilterRefresh_Click;
                br.Click += FilterRefresh_Click;
            }

            if (_btnApply is null && this.GetTemplateChild("BtnApply") is Button ba)
            {
                _btnApply = ba;
                ba.Click -= FilterAccept_Click;
                ba.Click += FilterAccept_Click;
            }
        }

        public void OnOpening()
        {
            if (_categoryList.ItemsSource is List<UnicodeCategoryModel> list
                && SourceCategories == _listSrc)
            {
                //foreach (var item in list)
                //    item.IsSelected = true;
            }
            else
            {
                _listSrc = SourceCategories;
                _categoryList.ItemsSource = Unicode.CreateCategoriesList(SourceCategories as List<UnicodeCategoryModel>);
            }
        }

        private void FilterAccept_Click(object sender, RoutedEventArgs e)
        {
            AcceptClicked?.Invoke(this, (List<UnicodeCategoryModel>)_categoryList.ItemsSource);
            Flyout?.Hide();
        }

        private void FilterRefresh_Click(object sender, RoutedEventArgs e)
        {
            _categoryList.ItemsSource = Unicode.CreateCategoriesList(SourceCategories as List<UnicodeCategoryModel>);
        }

        private void FilterSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((List<UnicodeCategoryModel>)_categoryList.ItemsSource))
                item.IsSelected = true;
        }

        private void FilterClear_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((List<UnicodeCategoryModel>)_categoryList.ItemsSource))
                item.IsSelected = false;
        }
    }
}
