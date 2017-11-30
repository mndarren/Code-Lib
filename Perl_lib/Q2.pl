#!/usr/bin/perl -w
use strict;
use Modern::Perl; # include feature say

my ($avg, $count, $total) = (0,1,2);#for multiple sub to use
# @params: a ref of hash and a file handle
# @return: the result ref hash with average
sub add_data_to_hash {
   my $ref_hash = shift;
   my $fh_in = shift;
   foreach my $line (<$fh_in>) {
      next if m{^\s*$};
      my @temp_array = split(/\s+/, $line);
      my $key = shift @temp_array;
      if(! exists $ref_hash->{$key}){
         @{$ref_hash->{$key}}[$avg, $count,$total] = (0) x 3;
      }

      foreach(@temp_array) {
         $ref_hash->{$key}->[$total] += $_;
         $ref_hash->{$key}->[$count]++;
         push @{$ref_hash->{$key}}, $_;#do this just for test, don't need it in the real SW
      }
      $ref_hash->{$key}->[$avg] = $ref_hash->{$key}->[$total]/$ref_hash->{$key}->[$count];
   }
   return $ref_hash;
}

# @params: ref_hash
# @others: just printing out the key and average is OK, others for test
sub print_average{
   my $ref_hash = shift;
   foreach my $key (sort keys %{$ref_hash}){
      printf("%-15s: \$AVG=%.4f \t\$count=%-10d \t\$total=%.4f\n", 
         $key, $ref_hash->{$key}->[$avg], $ref_hash->{$key}->[$count], $ref_hash->{$key}->[$total]);
   }
}

sub main{
   my $count = 0;
   my $ref_hash;
   print "Please input a data file: ";
   while(<>){
      last if m{^\s*$};
      s/^\s*|\s*$//g;#trim both ends
      die "Cannot open $_: $!" if (!open my $fh_in, '<', $_);
      say "start adding data into hash...";
      $ref_hash = &add_data_to_hash($ref_hash, $fh_in);
      say "finished adding data!";
      #&print_average($ref_hash); #print out each file output is just for test
      print "Please input a data file: ";
   }
   &print_average($ref_hash);
}

main()
