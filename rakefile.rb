require "FileUtils"
require 'albacore'

task :default => [:clean, :build, :publish]

task :publish do
  FileUtils.copy_file 'src\bin\Release\STFUANDCODE.EXE', 'build\STFUANDCODE.EXE'
end

task :clean do
  FileUtils.rm_rf "build" if File.exists? "build"
  Dir.mkdir  "build"
end

msbuild :build do |msb|
  msb.properties :configuration => :Release
  msb.targets :Clean, :Build
  msb.solution = "src/STFUANDCODE.sln"
end