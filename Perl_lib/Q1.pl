#!/usr/bin/perl -w
use strict;
use Modern::Perl; # include feature say

# @params: a file handle
# @return: a ref of matrix
sub read_matrix {
   my $fh_in = shift;
   my @array_ref;
   my $index = 0;
   foreach my $line (<$fh_in>) {
      $array_ref[$index] = [split(/\s+/, $line)];
      $index++;
   }
   return \@array_ref;
}
# @params: 2 ref of matrix
# @function: matrix1 * matrix2
# @return: a ref of result matrix
sub cal_matrix_mult {
   my $ref_matrix1 = shift;
   my $ref_matrix2 = shift;
   #here should check (# of columns of matrix1 == # of rows of matrix2)
   die "The # of columns of matrix1 does not match the # of rows of matrix2: $!"
      if(@{$ref_matrix1->[0]} != @{$ref_matrix2});
   my $ref_result;
   my $num_of_rows = @{$ref_matrix1};
   my $num_of_columns = @{$ref_matrix1->[0]};
   for (my $i = 0; $i < $num_of_rows; $i++) {
      my (@new_row, $new_num);
      for(my $m = 0; $m < $num_of_columns; $m++) {
         $new_num = 0;
         for(my $j = 0; $j < $num_of_columns; $j++) {
            $new_num += $ref_matrix1->[$i]->[$j] * $ref_matrix2->[$j]->[$m];
         }
         $new_row[$m] = $new_num;
      }
      $ref_result->[$i] = \@new_row;
   }
   return $ref_result;
}
# @params: a ref of matrix
# @function: print out the matrix
sub print_matrix {
   $, = ' ';
   my $ref = shift;
   foreach my $ref_array (@{$ref}) {
      say @{$ref_array};
   }
}

# no main subroutine(just want to try this format)
sub main {
   print "Please enter a data file: ";
   my $file1 = <>; 
   $file1 =~ s/^\s*|\s*$//g;
   print "Please enter another data file: ";
   my $file2 = <>;
   $file2 =~ s/^\s*|\s*$//g;
   die "Cannot open [$file1]: $!" if (!open my $fh_in1, '<', $file1);
   die "Cannot open [$file2]: $!" if (!open my $fh_in2, '<', $file2);

   my $ref_matrix1 = &read_matrix($fh_in1);
   my $ref_matrix2 = &read_matrix($fh_in2);
   my $ref_result = &cal_matrix_mult($ref_matrix1,$ref_matrix2);

   print_matrix($ref_matrix1);
   print "\n   X\n\n";
   print_matrix($ref_matrix2);
   print "\n   \|\|\n\n";
   &print_matrix($ref_result);
}
main();
